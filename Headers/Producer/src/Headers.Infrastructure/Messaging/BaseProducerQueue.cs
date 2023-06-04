using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Diagnostics;
using Headers.Core.Extensions;
using Headers.Core.Messaging;
using Headers.Core.Messaging.Settings;
using Microsoft.Extensions.Logging;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;

namespace Headers.Infrastructure.Messaging;

public abstract class BaseProducerQueue<T> : IProducerQueue<T>
{
    private Lazy<IConnection>? _connection;
    private IModel? _channel;

    private readonly ILogger _logger;

    protected virtual string Exchange => string.Empty;
    protected virtual ConcurrentDictionary<string, object> Headers => new();

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
        if (string.IsNullOrWhiteSpace(Exchange))
            throw new ArgumentNullException(nameof(Exchange), "Exchange can not be null or empty");

        if (Headers.IsEmpty)
            throw new ArgumentNullException(nameof(Headers), "Headers can not be empty");

        if (_channel is not { IsOpen: true })
        {
            _channel = _connection!.Value.CreateModel();
            
            _channel.ExchangeDeclare(
                exchange: Exchange,
                type: ExchangeType.Headers,
                durable: true,
                arguments: ImmutableDictionary<string, object>.Empty);
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
                onRetry: (ex, retryCount, ctx) =>
                {
                    _logger.LogInformation("Retrying operation");
                });

        policy.Execute(() =>
        {
            var properties = _channel.CreateBasicProperties();
            properties.Headers = Headers;

            _channel.BasicPublish(
                exchange: Exchange,
                routingKey: string.Empty,
                basicProperties: properties,
                body: obj.ToBytes());
            
            _channel.ConfirmSelect();
            _channel.WaitForConfirmsOrDie(TimeSpan.FromSeconds(5));
        });
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