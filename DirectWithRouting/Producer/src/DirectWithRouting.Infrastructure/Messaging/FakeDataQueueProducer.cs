using DirectWithRouting.Core.Data;
using DirectWithRouting.Core.Messaging;
using DirectWithRouting.Core.Messaging.Settings;
using Microsoft.Extensions.Logging;

namespace DirectWithRouting.Infrastructure.Messaging;

public sealed class FakeDataQueueProducer : BaseQueueProducer<FakeData>, IFakeDataQueueProducer
{
    protected override string ExchangeName => "ex.direct-with-routing.example";
    protected override string QueueName => "q.direct-with-routing.example";

    public FakeDataQueueProducer(
            RabbitMqSettings settings, 
            ILogger<FakeDataQueueProducer> logger
        ) : base(settings, logger)
    {
        GenerateChannel();
    }
    
    public new void Dispose()
    {
        base.Dispose();
    }
}