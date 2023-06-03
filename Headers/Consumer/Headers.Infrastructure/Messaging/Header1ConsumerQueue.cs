using System.Collections.Concurrent;
using Headers.Core.Data;
using Headers.Core.Messaging;
using Headers.Core.Messaging.Settings;
using Microsoft.Extensions.Logging;

namespace Headers.Infrastructure.Messaging;

public sealed class Header1ConsumerQueue : BaseConsumerQueue<FakeData>, IHeader1ConsumerQueue
{
    protected override string Exchange => "ex.headers.example";
    protected override string Queue => "q.headers.header1";
    protected override ConcurrentDictionary<string, object> Headers
    {
        get
        {
            var headers = new ConcurrentDictionary<string, object>();
            headers.TryAdd("x-match", "any");
            headers.TryAdd("source", "BBC");
            headers.TryAdd("category", "sports");

            return headers;
        }
    }

    public Header1ConsumerQueue(
        RabbitMqSettings settings, 
        ILogger logger
        ) : base(settings, logger)
    {
        GenerateChannel();
    }

    public new void Dispose()
    {
        base.Dispose();
    }
}