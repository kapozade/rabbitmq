using Direct.Core.Data;
using Microsoft.Extensions.Logging;
using Direct.Core.Messaging;
using Direct.Core.Messaging.Settings;

namespace Direct.Infrastructure.Messaging;

public sealed class FakeDataQueueConsumer : BaseQueueConsumer<FakeData>, IFakeDataQueueConsumer
{
    protected override string QueueName => "q.direct.example";

    public FakeDataQueueConsumer(
        RabbitMqSettings rabbitMqSettings, 
        ILogger<FakeDataQueueConsumer> logger)
            : base(rabbitMqSettings, logger)
    {
        if (string.IsNullOrWhiteSpace(QueueName))
            throw new ArgumentNullException(nameof(QueueName), "Queue name can not be null or empty.");
        
        GenerateChannel();
    }
}