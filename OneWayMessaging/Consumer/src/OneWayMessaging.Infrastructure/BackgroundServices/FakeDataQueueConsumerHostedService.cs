using Infrastructure.Messaging;
using Microsoft.Extensions.Hosting;

namespace Infrastructure.BackgroundServices;

public sealed class FakeDataQueueConsumerHostedService : IHostedService
{
    private readonly IFakeDataQueue _fakeDataQueue;

    public FakeDataQueueConsumerHostedService(
        IFakeDataQueue fakeDataQueue
    )
    {
        _fakeDataQueue = fakeDataQueue;
        _fakeDataQueue.Subscribe(example =>
        {
            Console.WriteLine(example.ToString());
        });
    }
    
    public Task StartAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _fakeDataQueue.Dispose();
        return Task.CompletedTask;
    }
}
