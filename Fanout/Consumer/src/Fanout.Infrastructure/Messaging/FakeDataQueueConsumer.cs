using Fanout.Core.Data;
using Fanout.Core.Messaging;
using Fanout.Core.Messaging.Settings;
using Microsoft.Extensions.Logging;

namespace Fanout.Infrastructure.Messaging;

public sealed class FakeDataQueueConsumer : BaseQueueConsumer<FakeData>, IFakeDataQueueConsumer
{
    protected override string QueueName => "q.fanout.example";
    protected override string ExchangeName => "ex.fanout.example";

    public FakeDataQueueConsumer(
            RabbitMqSettings settings, 
            ILogger<FakeDataQueueConsumer> logger
        ) : base(settings, logger)
    {
        GenerateChannel();
    }

    public new void Dispose()
    {
        base.Dispose();
    }
}