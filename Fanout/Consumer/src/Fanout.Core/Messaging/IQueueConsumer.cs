namespace Fanout.Core.Messaging;

public interface IQueueConsumer<T> : IDisposable
{
    void Subscribe(Action<T> callBack);
}