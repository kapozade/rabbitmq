using System.Collections.Concurrent;
using Headers.Core.Data;
using Headers.Core.Messaging;
using Headers.Core.Messaging.Settings;
using Microsoft.Extensions.Logging;

namespace Headers.Infrastructure.Messaging;

public sealed class Header3ProducerQueue : BaseProducerQueue<FakeData>, IHeader3ProducerQueue
{
    protected override string Exchange => "ex.headers.example";

    protected override ConcurrentDictionary<string, object> Headers
    {
        get
        {
            var headers = new ConcurrentDictionary<string, object>();
            headers.TryAdd("source", "BBC");
            headers.TryAdd("category", "weather");

            return headers;
        }
    }
    
    public Header3ProducerQueue(
        RabbitMqSettings settings,
        ILogger<Header3ProducerQueue> logger) 
        : base(settings, logger)
    {
        GenerateChannel();
    }

    public new void Dispose()
    {
        base.Dispose();
    }
}