using Microsoft.Extensions.Hosting;
using WorkQueue.Core.Messaging.Settings;

namespace WorkQueue.Application.BackgroundServices;

public class YetAnotherFakeDataConsumerBackgroundService : IHostedService
{
    private readonly IYetAnotherFakeDataConsumer _yetAnotherFakeDataConsumer;

    public YetAnotherFakeDataConsumerBackgroundService(
        IYetAnotherFakeDataConsumer yetAnotherFakeDataConsumer
        )
    {
        _yetAnotherFakeDataConsumer = yetAnotherFakeDataConsumer;
        _yetAnotherFakeDataConsumer.Subscribe(data =>
        {
            Console.WriteLine("Yet Another: " + data);
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