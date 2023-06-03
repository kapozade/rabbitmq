using Microsoft.Extensions.Logging;
using Topic.Core.Data;
using Topic.Core.Messaging;
using Topic.Core.Messaging.Settings;

namespace Topic.Infrastructure.Messaging;

public class Topic1ProducerQueue : BaseProducerQueue<FakeData>, IFakeDataProducerQueue
{
    protected override string ExchangeName => "ex.topic.example";
    protected override string RoutingKey => "topic-1";

    public Topic1ProducerQueue(
            RabbitMqSettings settings, 
            ILogger<Topic1ProducerQueue> logger
        ) : base(settings, logger)
    {
        GenerateChannel();
    }

    public new void Dispose()
    {
        base.Dispose();
    }
}