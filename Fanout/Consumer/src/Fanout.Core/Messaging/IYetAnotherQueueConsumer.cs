using Fanout.Core.Data;

namespace Fanout.Core.Messaging;

public interface IYetAnotherQueueConsumer : IQueueConsumer<FakeData>
{
}