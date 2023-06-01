using System.Collections.Immutable;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using OneWayMessaging.Core.Extensions;
using OneWayMessaging.Core.Messaging;
using OneWayMessaging.Core.Messaging.Settings;
using RabbitMQ.Client;

namespace OneWayMessaging.Infrastructure.Messaging;

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
        if (_channel == null || !_channel.IsOpen)
            throw new UnreachableException("Channel not initialized.");

        _channel.BasicPublish(
            exchange: ExchangeName,
            routingKey: QueueName,
            body: obj.ToBytes()
            );

        _channel.ConfirmSelect();
        _channel.WaitForConfirmsOrDie(timeout: TimeSpan.FromSeconds(5));
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
