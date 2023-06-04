using Microsoft.Extensions.Logging;
using WorkQueue.Core.Data;
using WorkQueue.Core.Messaging;
using WorkQueue.Core.Messaging.Settings;

namespace WorkQueue.Infrastructure.Messaging;

public sealed class FakeDataProducer : BaseQueueProducer<FakeData>, IFakeDataProducer
{
    protected override string QueueName => "q.work-queue.example";

    public FakeDataProducer(
            RabbitMqSettings rabbitMqSettings,
            ILogger<FakeDataProducer> logger
        ) : base(rabbitMqSettings, logger)
    {
        GenerateChannel();
    }
}