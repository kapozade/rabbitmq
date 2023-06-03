using Microsoft.Extensions.Hosting;
using Topic.Core.Messaging;

namespace Topic.Application.BackgroundServices;

public sealed class Topic1DataConsumerBackgroundService : IHostedService
{
    private readonly ITopic1DataConsumerQueue _topic1DataConsumerQueue;

    public Topic1DataConsumerBackgroundService(
            ITopic1DataConsumerQueue topic1DataConsumerQueue
        )
    {
        _topic1DataConsumerQueue = topic1DataConsumerQueue;
        _topic1DataConsumerQueue.Subscribe(data =>
        {
            Console.WriteLine($"Topic-1: {data}");
        });
    }
    
    public Task StartAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("Topic 1 Consumer is starting");
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _topic1DataConsumerQueue.Dispose();
        return Task.CompletedTask;
    }
}