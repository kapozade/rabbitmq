using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Topic.Core.Data;
using Topic.Core.Messaging;

namespace Topic.Application.BackgroundServices;

public class Topic1DataProducerBackgroundService : IHostedService
{
    private readonly IFakeDataProducerQueue _fakeDataProducerQueue;
    private readonly ILogger<Topic1DataProducerBackgroundService> _logger;

    public Topic1DataProducerBackgroundService(
            IFakeDataProducerQueue fakeDataProducerQueue,
            ILogger<Topic1DataProducerBackgroundService> logger
            )
    {
        _fakeDataProducerQueue = fakeDataProducerQueue;
        _logger = logger;
    }
    
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                _fakeDataProducerQueue.Publish(new FakeData("topic-1", 1, 1.23m, true));
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception occured while publishing a message. Message {Message}", ex.Message);
            }

            await Task.Delay(2000, cancellationToken);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _fakeDataProducerQueue.Dispose();
        return Task.CompletedTask;
    }
}