namespace Infrastructure.Messaging.Core;

public interface IQueueConsumer<T> : IDisposable
{
    void GenerateChannel();
    void Subscribe(Action<T> callBack);
}