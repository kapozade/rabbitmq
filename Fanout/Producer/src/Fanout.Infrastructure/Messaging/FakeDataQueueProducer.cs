using Fanout.Core.Data;
using Fanout.Core.Messaging;
using Fanout.Core.Messaging.Settings;
using Microsoft.Extensions.Logging;

namespace Fanout.Infrastructure.Messaging;

public sealed class FakeDataQueueProducer : BaseQueueProducer<FakeData>, IFakeDataQueueProducer
{
    protected override string ExchangeName => "ex.fanout.example";

    public FakeDataQueueProducer(
            RabbitMqSettings settings, 
            ILogger<FakeDataQueueProducer> logger
        ) : base(settings, logger)
    {
        GenerateChannel();
    }
}