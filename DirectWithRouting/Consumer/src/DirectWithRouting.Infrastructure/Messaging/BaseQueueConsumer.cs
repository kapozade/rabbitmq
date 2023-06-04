using System.Collections.Immutable;
using System.Diagnostics;
using DirectWithRouting.Core.Extensions;
using DirectWithRouting.Core.Messaging;
using DirectWithRouting.Core.Messaging.Settings;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace DirectWithRouting.Infrastructure.Messaging;

public abstract class BaseQueueConsumer<T> : IQueueConsumer<T>
{
    private Lazy<IConnection>? _connection;
    private IModel? _channel;
    private readonly ILogger _logger;

    protected virtual string QueueName => string.Empty;
    protected virtual string ExchangeName => string.Empty;
    protected virtual bool AutoAck => false;
    
    protected BaseQueueConsumer(
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
    
    public void Subscribe(Action<T> callBack)
    {
        if (_channel == null)
            throw new UnreachableException("Channel is not initialized");
        
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += (_, args) =>
        {
            try
            {
                var data = args.Body.ToArray().ToObject<T>();
                callBack(data);
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception occurred. Message: {Message}", ex.Message);
            }
            
            _channel.BasicAck(args.DeliveryTag, true);
        };

        _channel.BasicConsume(
            queue: QueueName, 
            autoAck: AutoAck, 
            consumer);
    }

    protected void GenerateChannel()
    {
        if (string.IsNullOrWhiteSpace(QueueName))
            throw new ArgumentNullException(nameof(QueueName), "QueueName can not be null or empty.");
        
        if (string.IsNullOrWhiteSpace(ExchangeName))
            throw new ArgumentNullException(nameof(ExchangeName), "ExchangeName can not be null or empty.");

        if (_channel is not { IsOpen: true })
        {
            _channel = _connection!.Value.CreateModel();

            _channel.QueueDeclare(
                queue: QueueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: ImmutableDictionary<string, object>.Empty);
            
            _channel.ExchangeDeclare(
                exchange: ExchangeName,
                type: ExchangeType.Direct,
                durable: true,
                autoDelete: false,
                arguments: ImmutableDictionary<string, object>.Empty);
            
            _channel.QueueBind(
                exchange: ExchangeName,
                routingKey: QueueName,
                queue: QueueName,
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