using Microsoft.Extensions.Hosting;
using WorkQueue.Core.Messaging;

namespace WorkQueue.Application.BackgroundServices;

public class SecondaryIFakeDataConsumerBackgroundService : IHostedService
{
    private readonly IFakeDataQueueConsumer _fakeDataQueueConsumer;

    public SecondaryIFakeDataConsumerBackgroundService(
        IFakeDataQueueConsumer fakeDataQueueConsumer
    )
    {
        _fakeDataQueueConsumer = fakeDataQueueConsumer;
        _fakeDataQueueConsumer.Subscribe(fakeData =>
        {
            Console.WriteLine("Primary IYetAnotherFakeDataConsumer: " + fakeData);
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