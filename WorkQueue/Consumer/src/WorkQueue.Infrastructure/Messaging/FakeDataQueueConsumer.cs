using Microsoft.Extensions.Logging;
using WorkQueue.Core.Data;
using WorkQueue.Core.Messaging;
using WorkQueue.Core.Messaging.Settings;

namespace WorkQueue.Infrastructure.Messaging;

public sealed class FakeDataQueueConsumer : BaseQueueConsumer<FakeData>, IFakeDataQueueConsumer
{
    protected override string QueueName => "q.work-queue.example";
    protected override bool AutoAck => false; // Message is acked after processed.

    public FakeDataQueueConsumer(
            RabbitMqSettings rabbitMqSettings, 
            ILogger<FakeDataQueueConsumer> logger
        ) 
        : base(rabbitMqSettings, logger)
    {
        GenerateChannel();
    }
}