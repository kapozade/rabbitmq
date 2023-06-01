using Infrastructure.Messaging;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Infrastructure.BackgroundServices;

public sealed class FakeDataQueueConsumerHostedService : IHostedService
{
    private readonly IFakeDataQueue _fakeDataQueue;

    public FakeDataQueueConsumerHostedService(
        IFakeDataQueue fakeDataQueue,
        ILogger<FakeDataQueueConsumerHostedService> logger
        )
    {
        _fakeDataQueue = fakeDataQueue;
        _fakeDataQueue.Subscribe(example =>
        {
            try
            {
                Console.WriteLine(example.ToString());
            }
            catch (Exception ex)
            {
                logger.LogError("Error occurred. {Message}", ex.Message);
            }
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
