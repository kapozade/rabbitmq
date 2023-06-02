using Microsoft.Extensions.Hosting;
using WorkQueue.Core.Messaging;

namespace WorkQueue.Application.BackgroundServices;

public class SecondaryFakeDataConsumerBackgroundService : IHostedService
{
    private readonly IFakeDataQueueConsumer _fakeDataQueueConsumer;

    public SecondaryFakeDataConsumerBackgroundService(
        IFakeDataQueueConsumer fakeDataQueueConsumer
    )
    {
        _fakeDataQueueConsumer = fakeDataQueueConsumer;
        _fakeDataQueueConsumer.Subscribe(fakeData =>
        {
            Console.WriteLine("Secondary: " + fakeData);
        });
    }
    
    public Task StartAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _fakeDataQueueConsumer.Dispose();
        return Task.CompletedTask;    }
}