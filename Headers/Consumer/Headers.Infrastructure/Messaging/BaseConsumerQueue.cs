using System.Collections.Immutable;
using System.Diagnostics;
using Headers.Core.Extensions;
using Headers.Core.Messaging;
using Headers.Core.Messaging.Settings;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Headers.Infrastructure.Messaging;

public abstract class BaseConsumerQueue<T>  : IConsumerQueue<T>
{
    private Lazy<IConnection>? _connection;
    private IModel? _channel;

    private readonly ILogger _logger;

    protected virtual string Exchange => string.Empty;
    protected virtual string Queue => string.Empty;
    protected virtual Dictionary<string, object> Headers => new Dictionary<string, object>();
    protected virtual bool AutoAck => false;

    public BaseConsumerQueue(
            RabbitMqSettings settings,
            ILogger logger
        )
    {
        _logger = logger;

        if (_connection is not { IsValueCreated: true } || !_connection.Value.IsOpen)
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
        if (string.IsNullOrWhiteSpace(Exchange))
            throw new ArgumentNullException(nameof(Exchange), "Exchange can not be null or empty");
        
        if (string.IsNullOrWhiteSpace(Queue))
            throw new ArgumentNullException(nameof(Queue), "Exchange can not be null or empty");

        if (Headers.Count < 1)
            throw new ArgumentNullException(nameof(Headers), "Headers can not be empty");
        
        if (_channel is not { IsOpen: true })
        {
            _channel = _connection!.Value.CreateModel();
            
            _channel.BasicQos(
                prefetchSize: 0,
                prefetchCount: 1,
                global: false);
            
            _channel.ExchangeDeclare(
                exchange: Exchange,
                type: ExchangeType.Headers,
                durable: true,
                arguments: ImmutableDictionary<string, object>.Empty);

            _channel.QueueDeclare(
                queue: Queue,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: ImmutableDictionary<string, object>.Empty);
            
            _channel.QueueBind(
                queue: Queue,
                exchange: Exchange,
                routingKey: string.Empty,
                arguments: Headers);
        }
    }
    
    public void Subscribe(Action<T> callBack)
    {
        if (_channel == null)
            throw new UnreachableException("Channel not initialized");
        
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
        };
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