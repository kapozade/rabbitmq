using Microsoft.Extensions.Hosting;
using Direct.Core.Messaging;

namespace Direct.Application.BackgroundService;

public sealed class FakeDataQueueConsumerHostedService : IHostedService
{
    private readonly IFakeDataQueueConsumer _fakeDataQueueConsumer;

    public FakeDataQueueConsumerHostedService(
        IFakeDataQueueConsumer fakeDataQueueConsumer
    )
    {
        _fakeDataQueueConsumer = fakeDataQueueConsumer;
        _fakeDataQueueConsumer.Subscribe(example =>
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
        _fakeDataQueueConsumer.Dispose();
        return Task.CompletedTask;
    }
}
