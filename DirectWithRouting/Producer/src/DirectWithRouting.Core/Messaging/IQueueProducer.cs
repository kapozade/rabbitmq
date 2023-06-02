namespace DirectWithRouting.Core.Messaging;

public interface IQueueProducer<T> : IDisposable
{
    void Publish(T obj);
}