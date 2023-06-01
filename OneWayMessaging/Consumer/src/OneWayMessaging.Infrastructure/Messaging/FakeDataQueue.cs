using Infrastructure.BackgroundServices;
using Infrastructure.Messaging.Core;
using Infrastructure.Messaging.Core.Settings;
using Microsoft.Extensions.Logging;
using OneWayMessaging.Core.FakeData;

namespace Infrastructure.Messaging;

public sealed class FakeDataQueue : BaseQueueConsumer<FakeData>, IFakeDataQueue
{
    protected override string QueueName => "q.direct.example";

    public FakeDataQueue(
        RabbitMqSettings rabbitMqSettings, 
        ILogger<FakeDataQueueConsumerHostedService> logger)
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