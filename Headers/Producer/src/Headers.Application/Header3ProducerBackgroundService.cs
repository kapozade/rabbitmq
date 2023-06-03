using Headers.Core.Data;
using Headers.Core.Messaging;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Headers.Application;

public sealed class Header3ProducerBackgroundService : BackgroundService
{
    private readonly IHeader3ProducerQueue _producerQueue;
    private readonly ILogger<Header3ProducerBackgroundService> _logger;

    public Header3ProducerBackgroundService(
            IHeader3ProducerQueue producerQueue,
            ILogger<Header3ProducerBackgroundService> logger
            )
    {
        _producerQueue = producerQueue;
        _logger = logger;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                _producerQueue.Publish(new FakeData("BBC weather", 1, 1.23m, true));
            }
            catch (Exception e)
            {
                _logger.LogError("Exception occured while publishing a message. Message: {Message}", e.Message);
            }

            await Task.Delay(10000, stoppingToken);
        }    }
}