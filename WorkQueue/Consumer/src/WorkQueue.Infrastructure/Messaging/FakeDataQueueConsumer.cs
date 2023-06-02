using Microsoft.Extensions.Logging;
using WorkQueue.Core.FakeData;
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
        if (string.IsNullOrWhiteSpace(QueueName))
            throw new ArgumentNullException(nameof(QueueName), "Queue name can not be null or empty.");

        GenerateChannel();
    }

    public new void Dispose()
    {
        base.Dispose();
    }
}