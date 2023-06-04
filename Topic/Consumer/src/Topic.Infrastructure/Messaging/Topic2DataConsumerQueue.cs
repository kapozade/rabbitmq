using Microsoft.Extensions.Logging;
using Topic.Core.Data;
using Topic.Core.Messaging;
using Topic.Core.Messaging.Settings;

namespace Topic.Infrastructure.Messaging;

public sealed class Topic2DataConsumerQueue : BaseConsumerQueue<FakeData>, ITopic2DataConsumerQueue
{
    protected override string ExchangeName => "ex.topic.example";
    protected override string QueueName => "q.topic.example-2";
    protected override string RoutingKey => "topic2.#";

    public Topic2DataConsumerQueue(
        RabbitMqSettings settings, 
        ILogger<Topic2DataConsumerQueue> logger
        ) : base(settings, logger)
    {
        GenerateChannel();
    }
}