namespace Topic.Core.Messaging;

public interface IConsumerQueue<T> : IDisposable
{
    void Subscribe(Action<T> callBack);
}