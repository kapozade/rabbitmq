using Microsoft.Extensions.Hosting;
using Topic.Core.Messaging;

namespace Topic.Application.BackgroundServices;

public sealed class Topic1ConsumerQueueBackgroundService : IHostedService
{
    private readonly IFakeDataConsumerQueue _fakeDataConsumerQueue;

    public Topic1ConsumerQueueBackgroundService(
            IFakeDataConsumerQueue fakeDataConsumerQueue
        )
    {
        _fakeDataConsumerQueue = fakeDataConsumerQueue;
        _fakeDataConsumerQueue.Subscribe(data =>
        {
            Console.WriteLine($"Topic-1: {data}");
        });
    }
    
    public Task StartAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _fakeDataConsumerQueue.Dispose();
        return Task.CompletedTask;
    }
}