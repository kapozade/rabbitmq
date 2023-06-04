using Microsoft.Extensions.Hosting;
using WorkQueue.Core.Messaging;

namespace WorkQueue.Application.BackgroundServices;

public class PrimaryIYetAnotherFakeDataConsumerBackgroundService : IHostedService
{
    private readonly IYetAnotherFakeDataConsumer _yetAnotherFakeDataConsumer;

    public PrimaryIYetAnotherFakeDataConsumerBackgroundService(
        IYetAnotherFakeDataConsumer yetAnotherFakeDataConsumer
        )
    {
        _yetAnotherFakeDataConsumer = yetAnotherFakeDataConsumer;
        _yetAnotherFakeDataConsumer.Subscribe(data =>
        {
            Console.WriteLine("Yet Another Fake Data Consumer: " + data);
        });
    }
    
    public Task StartAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _yetAnotherFakeDataConsumer.Dispose();
        return Task.CompletedTask;
    }
}