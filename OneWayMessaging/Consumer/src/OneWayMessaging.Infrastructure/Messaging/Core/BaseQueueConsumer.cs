using System.Collections.Immutable;
using Infrastructure.Messaging.Core.Settings;
using OneWayMessaging.Core.Extensions;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Infrastructure.Messaging.Core;

public abstract class BaseQueueConsumer<T> : IQueueConsumer<T>
{
    private Lazy<IConnection>? _connection;
    private Lazy<IModel>? _channel;

    protected virtual bool AutoAck => true;
    protected virtual string QueueName => string.Empty;

    protected BaseQueueConsumer(
        RabbitMqSettings rabbitMqSettings
        )
    {
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

        _channel = new Lazy<IModel>(() =>
        {
            var channel = _connection.Value.CreateModel();
            channel.QueueDeclare(
                queue: QueueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: ImmutableDictionary<string, object>.Empty);

            return channel;
        });
    }

    public void Subscribe(Action<T> callBack)
    {
        var consumer = new EventingBasicConsumer(_channel!.Value);
        consumer.Received += (_, args) =>
        {
            var obj = args.Body.ToArray().ToObject<T>();
            callBack(obj);
        };

        _channel.Value.BasicConsume(
            queue: QueueName,
            autoAck: AutoAck,
            consumer
        );
    }

    public void Dispose()
    {
        _channel?.Value.Close();
        _channel?.Value.Dispose();
        _channel = null;
        
        _connection?.Value.Close();
        _connection?.Value.Dispose();
        _connection = null;

        GC.SuppressFinalize(this);
    }
}