using Fanout.Core.Messaging;
using Microsoft.Extensions.Hosting;

namespace Fanout.Application.BackgroundServices;

public sealed class YetAnotherQueueConsumerBackgroundService : IHostedService
{
    private readonly IYetAnotherQueueConsumer _yetAnotherQueueConsumer;

    public YetAnotherQueueConsumerBackgroundService(
            IYetAnotherQueueConsumer yetAnotherQueueConsumer
        )
    {
        _yetAnotherQueueConsumer = yetAnotherQueueConsumer;
        _yetAnotherQueueConsumer.Subscribe(data =>
        {
            Console.WriteLine($"YetAnotherConsumer: {data}");
        });
    }
    
    public Task StartAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _yetAnotherQueueConsumer.Dispose();
        return Task.CompletedTask;
    }
}