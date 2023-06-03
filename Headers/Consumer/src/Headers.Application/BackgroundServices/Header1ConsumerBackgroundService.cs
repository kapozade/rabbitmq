using Headers.Core.Messaging;
using Microsoft.Extensions.Hosting;

namespace Headers.Application.BackgroundServices;

public sealed class Header1ConsumerBackgroundService : IHostedService
{
    private readonly IHeader1ConsumerQueue _header1ConsumerQueue;

    public Header1ConsumerBackgroundService(
            IHeader1ConsumerQueue header1ConsumerQueue
        )
    {
        _header1ConsumerQueue = header1ConsumerQueue;
        _header1ConsumerQueue.Subscribe(data =>
        {
            Console.WriteLine($"Any [BBC or sports]: {data}");
        });
    }
    
    public Task StartAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _header1ConsumerQueue.Dispose();
        return Task.CompletedTask;
    }
}