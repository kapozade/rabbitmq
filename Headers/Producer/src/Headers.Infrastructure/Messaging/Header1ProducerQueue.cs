using System.Collections.Concurrent;
using Headers.Core.Data;
using Headers.Core.Messaging;
using Headers.Core.Messaging.Settings;
using Microsoft.Extensions.Logging;

namespace Headers.Infrastructure.Messaging;

public sealed class Header1ProducerQueue : BaseProducerQueue<FakeData>, IHeader1ProducerQueue
{
    protected override string Exchange => "ex.headers.example";

    protected override ConcurrentDictionary<string, object> Headers
    {
        get
        {
            var headers = new ConcurrentDictionary<string, object>();
            headers.TryAdd("source", "BBC");
            headers.TryAdd("category", "sports");

            return headers;
        }
    }

    public Header1ProducerQueue(
        RabbitMqSettings settings,
        ILogger<Header1ProducerQueue> logger
        ) : base(settings, logger)
    {
        GenerateChannel();
    }

    public new void Dispose()
    {
        base.Dispose();
    }
}