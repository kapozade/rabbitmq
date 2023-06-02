using Fanout.Core.Data;

namespace Fanout.Core.Messaging;

public interface IFakeDataQueueProducer : IQueueProducer<FakeData>
{
}