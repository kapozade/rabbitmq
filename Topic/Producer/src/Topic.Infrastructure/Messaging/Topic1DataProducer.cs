using Microsoft.Extensions.Logging;
using Topic.Core.Data;
using Topic.Core.Messaging;
using Topic.Core.Messaging.Settings;

namespace Topic.Infrastructure.Messaging;

public sealed class Topic1DataProducer : BaseProducerQueue<FakeData>, ITopic1DataProducer
{
    protected override string Exchange => "ex.topic.example";
    protected override string Routing => "topic1";

    public Topic1DataProducer(
            RabbitMqSettings settings, 
            ILogger<Topic1DataProducer> logger
        ) : base(settings, logger)
    {
        GenerateChannel();
    }
}