using Fanout.Core.Data;

namespace Fanout.Core.Messaging;

public interface IFakeDataQueueConsumer : IQueueConsumer<FakeData>
{
}