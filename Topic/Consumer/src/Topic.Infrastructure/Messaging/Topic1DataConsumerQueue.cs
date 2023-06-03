using Microsoft.Extensions.Logging;
using Topic.Core.Data;
using Topic.Core.Messaging;
using Topic.Core.Messaging.Settings;

namespace Topic.Infrastructure.Messaging;

public sealed class Topic1ConsumerQueue : BaseConsumerQueue<FakeData>, ITopic1ConsumerQueue
{
    protected override string ExchangeName => "ex.topic.example";
    protected override string QueueName => "q.topic.example-1";
    protected override string RoutingKey => "#.topic-1";

    public Topic1ConsumerQueue(
        RabbitMqSettings settings, 
        ILogger<Topic1ConsumerQueue> logger
        ) : base(settings, logger)
    {
        GenerateChannel();
    }

    public new void Dispose()
    {
        base.Dispose();
    }
}