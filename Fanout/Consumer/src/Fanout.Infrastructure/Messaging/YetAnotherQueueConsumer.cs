using Fanout.Core.Data;
using Fanout.Core.Messaging;
using Fanout.Core.Messaging.Settings;
using Microsoft.Extensions.Logging;

namespace Fanout.Infrastructure.Messaging;

public class YetAnotherQueueConsumer : BaseQueueConsumer<FakeData>, IYetAnotherQueueConsumer
{
    protected override string QueueName => "q.fanout.example2";
    protected override string ExchangeName => "ex.fanout.example";

    public YetAnotherQueueConsumer(
        RabbitMqSettings settings, 
        ILogger<YetAnotherQueueConsumer> logger
        ) : base(settings, logger)
    {
        GenerateChannel();
    }

    public new void Dispose()
    {
        base.Dispose();
    }
}