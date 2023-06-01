using System.Collections.Immutable;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Direct.Core.Extensions;
using Direct.Core.Messaging;
using Direct.Core.Messaging.Settings;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;

namespace Direct.Infrastructure.Messaging;

public abstract class BaseQueueProducer<T> : IQueueProducer<T>
{
    private readonly ILogger _logger;
    private Lazy<IConnection>? _connection;
    private IModel? _channel;

    protected virtual string ExchangeName => string.Empty;
    protected virtual string QueueName => string.Empty;
    
    protected BaseQueueProducer(
        RabbitMqSettings rabbitMqSettings, 
        ILogger logger
        )
    {
        _logger = logger;

        if (_connection == null || !_connection.Value.IsOpen)
        {
            _connection = new Lazy<IConnection>(() =>
            {
                var connectionFactory = new ConnectionFactory
                {
                    UserName = rabbitMqSettings.Username,
                    Password = rabbitMqSettings.Password,
                    HostName = rabbitMqSettings.Host,
                    Port = rabbitMqSettings.Port,
                    VirtualHost = rabbitMqSettings.VirtualHost
                };

                return connectionFactory.CreateConnection();
            });
        }
    }
 
    protected void GenerateChannel()
    {
        if (_channel is not { IsOpen: true })
        {
            _channel = _connection!.Value.CreateModel();
            _channel.QueueDeclare(
                queue: QueueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: ImmutableDictionary<string, object>.Empty);
        }
    }

    public void Publish(T obj)
    {
        if (_channel == null)
            throw new UnreachableException("Channel not initialized.");

        var policy = Policy.Handle<OperationInterruptedException>()
            .WaitAndRetry(3, 
                retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), 
                onRetry: (ex, retryCount, context) =>
            {
                {
                    _logger.LogInformation("Retrying operation");
                }
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

    public void Dispose()
    {
        _channel?.Close();
        _channel?.Dispose();
        _channel = null;

        if (_connection is { IsValueCreated: true })
        {
            _connection?.Value.Close();
            _connection?.Value.Dispose();
            _connection = null;
        }

        GC.SuppressFinalize(this);
    }
}
