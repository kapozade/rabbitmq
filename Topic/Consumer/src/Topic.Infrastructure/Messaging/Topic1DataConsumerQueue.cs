using Microsoft.Extensions.Logging;
using Topic.Core.Data;
using Topic.Core.Messaging;
using Topic.Core.Messaging.Settings;

namespace Topic.Infrastructure.Messaging;

public sealed class Topic1DataConsumerQueue : BaseConsumerQueue<FakeData>, ITopic1DataConsumerQueue
{
    protected override string ExchangeName => "ex.topic.example";
    protected override string QueueName => "q.topic.example-1";
    protected override string RoutingKey => "topic1.#";

    public Topic1DataConsumerQueue(
        RabbitMqSettings settings, 
        ILogger<Topic1DataConsumerQueue> logger
        ) : base(settings, logger)
    {
        GenerateChannel();
    }

    public new void Dispose()
    {
        base.Dispose();
    }
}