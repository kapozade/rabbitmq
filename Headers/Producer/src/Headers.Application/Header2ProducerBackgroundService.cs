using Headers.Core.Data;
using Headers.Core.Messaging;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Headers.Application;

public sealed class Header2ProducerBackgroundService : BackgroundService
{
    private readonly IHeader2ProducerQueue _producerQueue;
    private readonly ILogger<Header2ProducerBackgroundService> _logger;

    public Header2ProducerBackgroundService(
            IHeader2ProducerQueue producerQueue,
            ILogger<Header2ProducerBackgroundService> logger
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
                _producerQueue.Publish(new FakeData("Header 2", 1, 1.23m, true));
            }
            catch (Exception e)
            {
                _logger.LogError("Exception occured while publishing a message. Message: {Message}", e.Message);
            }

            await Task.Delay(10000, stoppingToken);
        }
    }
}