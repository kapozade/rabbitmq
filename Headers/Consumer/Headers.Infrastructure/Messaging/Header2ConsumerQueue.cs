using System.Collections.Concurrent;
using Headers.Core.Data;
using Headers.Core.Messaging;
using Headers.Core.Messaging.Settings;
using Microsoft.Extensions.Logging;

namespace Headers.Infrastructure.Messaging;

public class Header2ConsumerQueue : BaseConsumerQueue<FakeData>, IHeader2ConsumerQueue
{
    protected override string Exchange => "ex.headers.example";
    protected override string Queue => "q.headers.header2";
    protected override ConcurrentDictionary<string, object> Headers
    {
        get
        {
            var headers = new ConcurrentDictionary<string, object>();
            headers.TryAdd("x-match", "all");
            headers.TryAdd("source", "CNN");
            headers.TryAdd("category", "sports");

            return headers;
        }
    }
    
    public Header2ConsumerQueue(
            RabbitMqSettings settings,
            ILogger<Header2ConsumerQueue> logger
        ) : base(settings, logger)
    {
        GenerateChannel();
    }
    
    public new void Dispose()
    {
        base.Dispose();
    }
}