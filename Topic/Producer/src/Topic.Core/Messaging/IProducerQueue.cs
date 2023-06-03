namespace Topic.Core.Messaging;

public interface IProducerQueue<T> : IDisposable
{
    void Publish(T obj);
}