using Headers.Core.Messaging;
using Microsoft.Extensions.Hosting;

namespace Headers.Application.BackgroundServices;

public sealed class Header2ConsumerBackgroundService : IHostedService
{
    private readonly IHeader2ConsumerQueue _header2ConsumerQueue;

    public Header2ConsumerBackgroundService(
            IHeader2ConsumerQueue header2ConsumerQueue
    )
    {
        _header2ConsumerQueue = header2ConsumerQueue;
        _header2ConsumerQueue.Subscribe(data =>
        {
            Console.WriteLine($"ALL [CNN sports] {data}");
        });
    }
    
    public Task StartAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _header2ConsumerQueue.Dispose();
        return Task.CompletedTask;
    }
}