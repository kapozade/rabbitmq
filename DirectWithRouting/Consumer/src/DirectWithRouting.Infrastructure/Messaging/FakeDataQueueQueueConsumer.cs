using DirectWithRouting.Core.Data;
using DirectWithRouting.Core.Messaging;
using DirectWithRouting.Core.Messaging.Settings;
using Microsoft.Extensions.Logging;

namespace DirectWithRouting.Infrastructure.Messaging;

public sealed class FakeDataQueueQueueConsumer : BaseQueueConsumer<FakeData>, IFakeDataQueueConsumer
{
    protected override string ExchangeName => "ex.direct-with-routing.example";
    protected override string QueueName => "q.direct-with-routing.example";

    public FakeDataQueueQueueConsumer(
        RabbitMqSettings settings,
        ILogger<FakeDataQueueQueueConsumer> logger
        ) : base(settings, logger)
    {
        GenerateChannel();
    }
}