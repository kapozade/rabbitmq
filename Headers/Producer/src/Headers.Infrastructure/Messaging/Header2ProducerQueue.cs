using System.Collections.Concurrent;
using Headers.Core.Data;
using Headers.Core.Messaging;
using Headers.Core.Messaging.Settings;
using Microsoft.Extensions.Logging;

namespace Headers.Infrastructure.Messaging;

public sealed class Header2ProducerQueue : BaseProducerQueue<FakeData>, IHeader2ProducerQueue
{
    protected override string Exchange => "ex.headers.example";

    protected override ConcurrentDictionary<string, object> Headers
    {
        get
        {
            var headers = new ConcurrentDictionary<string, object>();
            headers.TryAdd("source", "CNN");
            headers.TryAdd("category", "sports");

            return headers;
        }
    }

    public Header2ProducerQueue(
        RabbitMqSettings settings, 
        ILogger<Header2ProducerQueue> logger
        ) : base(settings, logger)
    {
        GenerateChannel();
    }
}