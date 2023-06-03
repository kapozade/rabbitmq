using Microsoft.Extensions.Logging;
using Topic.Core.Data;
using Topic.Core.Messaging;
using Topic.Core.Messaging.Settings;

namespace Topic.Infrastructure.Messaging;

public sealed class Topic1DataProducerQueue : BaseProducerQueue<FakeData>, ITopic1DataProducerQueue
{
    protected override string ExchangeName => "ex.topic.example";
    protected override string RoutingKey => "topic1.#";

    public Topic1DataProducerQueue(
            RabbitMqSettings settings, 
            ILogger<Topic1DataProducerQueue> logger
        ) : base(settings, logger)
    {
        GenerateChannel();
    }

    public new void Dispose()
    {
        base.Dispose();
    }
}