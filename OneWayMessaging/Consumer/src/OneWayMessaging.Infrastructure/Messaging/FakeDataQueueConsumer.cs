using Microsoft.Extensions.Logging;
using OneWayMessaging.Core.FakeData;
using OneWayMessaging.Core.Messaging;
using OneWayMessaging.Core.Messaging.Settings;

namespace Infrastructure.Messaging;

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

    public new void Dispose()
    {
        base.Dispose();
    }
}