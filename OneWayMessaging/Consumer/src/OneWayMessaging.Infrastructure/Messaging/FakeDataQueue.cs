using Infrastructure.Messaging.Core;
using Infrastructure.Messaging.Core.Settings;
using OneWayMessaging.Core.FakeData;

namespace Infrastructure.Messaging;

public sealed class FakeDataQueue : BaseQueueConsumer<FakeData>, IFakeDataQueue
{
    protected override string QueueName => "q.direct.example";

    public FakeDataQueue(RabbitMqSettings rabbitMqSettings) : base(rabbitMqSettings)
    {
        if (string.IsNullOrWhiteSpace(QueueName))
            throw new ArgumentNullException(nameof(QueueName), "Queue name can not be null or empty.");
    }

    public new void Dispose()
    {
        base.Dispose();
    }
}