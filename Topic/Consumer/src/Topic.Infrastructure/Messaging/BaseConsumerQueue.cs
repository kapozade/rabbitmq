using System.Collections.Immutable;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Topic.Core.Extensions;
using Topic.Core.Messaging;
using Topic.Core.Messaging.Settings;

namespace Topic.Infrastructure.Messaging;

public abstract class BaseConsumerQueue<T> : IConsumerQueue<T>
{
    private Lazy<IConnection>? _connection;
    private IModel? _channel;
    private readonly ILogger _logger;

    protected virtual string QueueName => string.Empty;
    protected virtual string ExchangeName => string.Empty;
    protected virtual string RoutingKey => string.Empty;
    protected virtual bool AutoAck => false;
    
    protected BaseConsumerQueue(
        RabbitMqSettings settings,
        ILogger logger
        )
    {
        _logger = logger;

        if (_connection is not { IsValueCreated: true }
            || !_connection.Value.IsOpen
           )
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
        if (string.IsNullOrWhiteSpace(QueueName))
            throw new ArgumentNullException(nameof(QueueName), "QueueName can not be null or empty");
        
        if (string.IsNullOrWhiteSpace(ExchangeName))
            throw new ArgumentNullException(nameof(ExchangeName), "ExchangeName can not be null or empty");

        if(string.IsNullOrWhiteSpace(RoutingKey))
            throw new ArgumentNullException(nameof(RoutingKey), "RoutingKey can not be null or empty");
        
        if (_channel is not { IsOpen: true })
        {
            _channel = _connection!.Value.CreateModel();

            _channel.BasicQos(
                prefetchSize: 0,
                prefetchCount: 1,
                global: false);
            
            _channel.ExchangeDeclare(
                exchange: ExchangeName,
                type: ExchangeType.Topic,
                durable: true,
                autoDelete: false,
                arguments: ImmutableDictionary<string, object>.Empty
                );

            _channel.QueueDeclare(
                queue: QueueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: ImmutableDictionary<string, object>.Empty);
            
            _channel.QueueBind(
                exchange: ExchangeName,
                queue: QueueName,
                routingKey: RoutingKey,
                arguments: ImmutableDictionary<string, object>.Empty);
        }
    }
    
    public void Subscribe(Action<T> callBack)
    {
        if (_channel == null)
            throw new UnreachableException("Channel is not initialized.");

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
                _logger.LogError("Exception occured while processing message. Message: {Message}", ex.Message);
            }
            
            _channel.BasicAck(args.DeliveryTag, true);
        };

        _channel.BasicConsume(
            queue: QueueName, 
            autoAck: AutoAck, 
            consumer);
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