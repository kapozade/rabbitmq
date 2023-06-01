namespace Infrastructure.Messaging.Core;

public interface IQueueConsumer<T> : IDisposable
{
    void Subscribe(Action<T> callBack);
}