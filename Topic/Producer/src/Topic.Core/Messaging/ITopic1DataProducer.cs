using Topic.Core.Data;

namespace Topic.Core.Messaging;

public interface ITopic1DataProducer : IProducerQueue<FakeData>
{
}