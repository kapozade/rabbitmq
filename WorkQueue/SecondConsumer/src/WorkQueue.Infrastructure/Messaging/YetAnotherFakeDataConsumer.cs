using Microsoft.Extensions.Logging;
using WorkQueue.Core.Data;
using WorkQueue.Core.Messaging;
using WorkQueue.Core.Messaging.Settings;

namespace WorkQueue.Infrastructure.Messaging;

public sealed class YetAnotherFakeDataConsumer : BaseQueueConsumer<FakeData>, IYetAnotherFakeDataConsumer
{
    protected override string QueueName => "q.work-queue.example";
    protected override bool AutoAck => false; // Message is acked after processed.
    
    public YetAnotherFakeDataConsumer(           RabbitMqSettings settings, 
        ILogger<FakeDataQueueConsumer> logger
    ) : base(settings, logger)
    {
        GenerateChannel();
    }
}