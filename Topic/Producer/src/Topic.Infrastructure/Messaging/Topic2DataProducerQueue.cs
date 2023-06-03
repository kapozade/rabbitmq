using Microsoft.Extensions.Logging;
using Topic.Core.Data;
using Topic.Core.Messaging;
using Topic.Core.Messaging.Settings;

namespace Topic.Infrastructure.Messaging;

public sealed class Topic2DataProducerQueue : BaseProducerQueue<FakeData>, ITopic2DataProducerQueue
{
    protected override string ExchangeName => "ex.topic.example";
    protected override string RoutingKey => "topic2.#";

    public Topic2DataProducerQueue(
            RabbitMqSettings settings, 
            ILogger<Topic2DataProducerQueue> logger
        ) : base(settings, logger)
    {
        GenerateChannel();
    }

    public new void Dispose()
    {
        base.Dispose();
    }
}