using Infrastructure.Messaging.Core;
using OneWayMessaging.Core.FakeData;

namespace Infrastructure.Messaging;

public interface IFakeDataQueue : IQueueConsumer<FakeData>
{
}