using System.Collections.Immutable;
using Infrastructure.Messaging.Core.Settings;
using Microsoft.Extensions.Logging;
using OneWayMessaging.Core.Extensions;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Infrastructure.Messaging.Core;

public abstract class BaseQueueConsumer<T> : IQueueConsumer<T>
{
    private readonly ILogger _logger;
    private Lazy<IConnection>? _connection;
    private IModel? _channel;

    protected virtual bool AutoAck => true;
    protected virtual string QueueName => string.Empty;

    protected BaseQueueConsumer(
        RabbitMqSettings rabbitMqSettings,
        ILogger logger)
    {
        _logger = logger;

        if (_connection?.Value is not { IsOpen: true })
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

    public void Subscribe(Action<T> callBack)
    {
        if (_channel is not { IsOpen: true })
            throw new Exception("Channel is not initialized.");
        
        var consumer = new EventingBasicConsumer(_channel!);
        consumer.Received += (_, args) =>
        {
            try
            {
                var obj = args.Body.ToArray().ToObject<T>();
                callBack(obj);
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception occurred. {Message}", ex.Message);
            }
        };

        _channel.BasicConsume(
            queue: QueueName,
            autoAck: AutoAck,
            consumer
        );
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