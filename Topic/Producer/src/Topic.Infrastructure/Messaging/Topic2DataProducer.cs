using Microsoft.Extensions.Logging;
using Topic.Core.Data;
using Topic.Core.Messaging;
using Topic.Core.Messaging.Settings;

namespace Topic.Infrastructure.Messaging;

public sealed class Topic2DataProducer : BaseProducerQueue<FakeData>, ITopic2DataProducer
{
    protected override string Exchange => "ex.topic.example";
    protected override string Routing => "topic2";

    public Topic2DataProducer(
            RabbitMqSettings settings, 
            ILogger logger
        ) : base(settings, logger)
    {
        GenerateChannel();
    }

    public new void Dispose()
    {
        base.Dispose();
    }
}