using System.Collections.Immutable;
using System.Diagnostics;
using DirectWithRouting.Core.Extensions;
using DirectWithRouting.Core.Messaging;
using DirectWithRouting.Core.Messaging.Settings;
using Microsoft.Extensions.Logging;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;

namespace DirectWithRouting.Infrastructure.Messaging;

public abstract class BaseQueueProducer<T> : IQueueProducer<T>
{
    private Lazy<IConnection>? _connection;
    private IModel? _channel;
    private readonly ILogger _logger;
    
    protected virtual string QueueName => string.Empty;
    protected virtual string ExchangeName => string.Empty;

    protected BaseQueueProducer(
            RabbitMqSettings settings,
            ILogger logger
        )
    {
        _logger = logger;
        
        if (_connection is not { IsValueCreated: true } 
            || !_connection.Value.IsOpen)
        {
            _connection = new Lazy<IConnection>(() =>
            {
                var connectionFactory = new ConnectionFactory
                {
                    UserName = settings.Username,
                    Password = settings.Password,
                    HostName = settings.Host,
                    VirtualHost = settings.VirtualHost,
                    Port = settings.Port
                };

                return connectionFactory.CreateConnection();
            });
        }
    }
    
    public void Publish(T obj)
    {
        if (_channel == null)
            throw new UnreachableException("Channel is not initialized");

        var policy = Policy
            .Handle<OperationInterruptedException>()
            .WaitAndRetry(3,
                retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                onRetry: (ex, retryCount, context) =>
                {
                    _logger.LogInformation("Retrying operation.");
                });

        policy.Execute(() =>
        {
            _channel.BasicPublish(
                exchange: ExchangeName,
                routingKey: QueueName,
                body: obj.ToBytes()
            );
            
            _channel.ConfirmSelect();
            _channel.WaitForConfirmsOrDie(timeout: TimeSpan.FromSeconds(5));
        });
    }

    protected void GenerateChannel()
    {
        if (string.IsNullOrWhiteSpace(QueueName))
            throw new ArgumentNullException(nameof(QueueName), "QueueName can not be null or empty");
        
        if (string.IsNullOrWhiteSpace(ExchangeName))
            throw new ArgumentNullException(nameof(ExchangeName), "ExchangeName can not be null or empty");

        if (_channel is not { IsOpen: true })
        {
            _channel = _connection!.Value.CreateModel();
            
            _channel.ExchangeDeclare(
                exchange: ExchangeName,
                type: ExchangeType.Direct,
                durable: true,
                autoDelete: false,
                arguments: ImmutableDictionary<string, object>.Empty);

            _channel.QueueDeclare(
                queue: QueueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: ImmutableDictionary<string, object>.Empty);
            
            _channel.QueueBind(
                queue: QueueName,
                exchange: ExchangeName,
                routingKey: QueueName,
                arguments: ImmutableDictionary<string, object>.Empty);
        }
    }

    public virtual void Dispose()
    {
        _channel?.Close();
        _channel?.Dispose();
        _channel = null;

        if (_connection is { IsValueCreated: true })
        {
            _connection.Value.Close();
            _connection.Value.Dispose();
            _connection = null;
        }
        
        GC.SuppressFinalize(this); 
    }
}