using WorkQueue.Core.Data;

namespace WorkQueue.Core.Messaging;

public interface IFakeDataProducer: IQueueProducer<FakeData>
{
}