using Microsoft.Extensions.Hosting;
using Topic.Core.Messaging;

namespace Topic.Application.BackgroundServices;

public sealed class Topic2DataConsumerBackgroundService : IHostedService
{
    private readonly ITopic2DataConsumerQueue _fakeDataConsumerQueue;

    public Topic2DataConsumerBackgroundService(
            ITopic2DataConsumerQueue fakeDataConsumerQueue
        )
    {
        _fakeDataConsumerQueue = fakeDataConsumerQueue;
        _fakeDataConsumerQueue.Subscribe(data =>
        {
            Console.WriteLine($"Topic-2: {data}");
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