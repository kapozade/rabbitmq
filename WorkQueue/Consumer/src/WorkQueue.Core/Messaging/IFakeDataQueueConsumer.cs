using WorkQueue.Core.Data;

namespace WorkQueue.Core.Messaging;

public interface IFakeDataQueueConsumer : IQueueConsumer<FakeData> 
{
}