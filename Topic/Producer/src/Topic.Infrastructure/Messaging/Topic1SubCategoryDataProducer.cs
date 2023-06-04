using Microsoft.Extensions.Logging;
using Topic.Core.Data;
using Topic.Core.Messaging;
using Topic.Core.Messaging.Settings;

namespace Topic.Infrastructure.Messaging;

public sealed class Topic1SubCategoryDataProducer : BaseProducerQueue<FakeData>, ITopic1SubCategoryDataProducer
{
    protected override string Exchange => "ex.topic.example";
    protected override string Routing => "topic1.sub";

    public Topic1SubCategoryDataProducer(
            RabbitMqSettings settings, 
            ILogger<Topic1SubCategoryDataProducer> logger
        ) : base(settings, logger)
    {
        GenerateChannel();
    }
}