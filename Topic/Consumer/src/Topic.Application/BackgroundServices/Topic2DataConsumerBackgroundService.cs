using Microsoft.Extensions.Hosting;
using Topic.Core.Messaging;

namespace Topic.Application.BackgroundServices;

public sealed class Topic2DataConsumerBackgroundService : IHostedService
{
    private readonly ITopic2DataConsumerQueue _topic2DataConsumerQueue;

    public Topic2DataConsumerBackgroundService(
            ITopic2DataConsumerQueue topic2DataConsumerQueue
        )
    {
        _topic2DataConsumerQueue = topic2DataConsumerQueue;
        _topic2DataConsumerQueue.Subscribe(data =>
        {
            Console.WriteLine($"Topic-2: {data}");
        });
    }
    
    public Task StartAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("Topic 2 Consumer is starting");
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _topic2DataConsumerQueue.Dispose();
        return Task.CompletedTask;
    }
}