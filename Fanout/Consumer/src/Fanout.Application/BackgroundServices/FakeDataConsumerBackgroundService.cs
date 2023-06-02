using Fanout.Core.Messaging;
using Microsoft.Extensions.Hosting;

namespace Fanout.Application.BackgroundServices;

public class FakeDataConsumerBackgroundService : IHostedService
{
    private readonly IFakeDataQueueConsumer _fakeDataQueueConsumer;

    public FakeDataConsumerBackgroundService(
            IFakeDataQueueConsumer fakeDataQueueConsumer
        )
    {
        _fakeDataQueueConsumer = fakeDataQueueConsumer;
        _fakeDataQueueConsumer.Subscribe(data =>
        {
            Console.WriteLine($"FakeDataConsumer: {data}");
        });
    }
    
    public Task StartAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _fakeDataQueueConsumer.Dispose();
        return Task.CompletedTask;
    }
}