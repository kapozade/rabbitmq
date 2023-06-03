using System.Collections.Immutable;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using Topic.Core.Extensions;
using Topic.Core.Messaging;
using Topic.Core.Messaging.Settings;

namespace Topic.Infrastructure.Messaging;

public abstract class BaseProducerQueue<T> : IProducerQueue<T>
{
    private Lazy<IConnection>? _connection;
    private IModel? _channel;
    private readonly ILogger _logger;

    protected virtual string ExchangeName => string.Empty;
    protected virtual string RoutingKey => string.Empty;

    protected BaseProducerQueue(
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

    protected void GenerateChannel()
    {
        if (string.IsNullOrWhiteSpace(ExchangeName))
            throw new ArgumentNullException(nameof(ExchangeName), "ExchangeName can not be null or empty");
        
        if(string.IsNullOrEmpty(RoutingKey))
            throw new ArgumentNullException(nameof(RoutingKey), "RoutingKey can not be null or empty");
        
        if (_channel is not { IsOpen: true })
        {
            _channel = _connection!.Value.CreateModel();

            _channel.ExchangeDeclare(
                exchange: ExchangeName,
                type: ExchangeType.Topic,
                durable: true,
                autoDelete: false,
                arguments: ImmutableDictionary<string, object>.Empty
            );
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
                    _logger.LogInformation("Retrying operation");
                });

        policy.Execute(() =>
        {
            _channel.BasicPublish(
                exchange: ExchangeName,
                routingKey: RoutingKey,
                body: obj.ToBytes());

            _channel.ConfirmSelect();
            _channel.WaitForConfirmsOrDie(TimeSpan.FromSeconds(5));
        });
    }

    public void Dispose()
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