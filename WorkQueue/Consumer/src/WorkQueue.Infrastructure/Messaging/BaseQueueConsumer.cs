using System.Collections.Immutable;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using WorkQueue.Core.Extensions;
using WorkQueue.Core.Messaging;
using WorkQueue.Core.Messaging.Settings;

namespace WorkQueue.Infrastructure.Messaging;

public abstract class BaseQueueConsumer<T> : IQueueConsumer<T>
{
    private readonly ILogger _logger;
    private Lazy<IConnection>? _connection;
    private IModel? _channel;

    protected virtual bool AutoAck => false;
    protected virtual string QueueName => string.Empty;

    protected BaseQueueConsumer(
        RabbitMqSettings rabbitMqSettings,
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
        if (string.IsNullOrWhiteSpace(QueueName))
            throw new ArgumentNullException(nameof(QueueName), "Queue name can not be null or empty.");

        if (_channel is not { IsOpen: true })
        {
            _channel = _connection!.Value.CreateModel();

            _channel.BasicQos(
                prefetchSize: 0,
                prefetchCount: 1,
                global: false);

            _channel.QueueDeclare(
                queue: QueueName,
                durable: true,
                exclusive: false,
                autoDelete:false,
                arguments: ImmutableDictionary<string, object>.Empty);
        }
    }
    
    public void Subscribe(Action<T> callBack)
    {
        if (_channel is not { IsOpen: true })
            throw new Exception("Channel is not initialized.");

        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += (_, args) =>
        {
            try
            {
                var obj = args.Body.ToArray().ToObject<T>();
                callBack(obj);
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception occurred. Message: {Message}", ex.Message);
            }

            // Comment below statement if you want to send ack after you processed the message.
            // Do not forget to set AutoAck as true as well..
            _channel.BasicAck(args.DeliveryTag, multiple: true);
        };

        _channel.BasicConsume(
            queue: QueueName,
            autoAck: AutoAck,
            consumer: consumer);
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