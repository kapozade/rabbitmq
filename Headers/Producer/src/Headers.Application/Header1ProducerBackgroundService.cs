using Headers.Core.Data;
using Headers.Core.Messaging;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Headers.Application;

public sealed class Header1ProducerBackgroundService : BackgroundService
{
    private readonly IHeader1ProducerQueue _producerQueue;
    private readonly ILogger<Header1ProducerBackgroundService> _logger;

    public Header1ProducerBackgroundService(
            IHeader1ProducerQueue producerQueue,
            ILogger<Header1ProducerBackgroundService> logger
        )
    {
        _producerQueue = producerQueue;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Console.WriteLine("Header 1 BS starting");
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                _producerQueue.Publish(new FakeData("Header 1", 1, 1.23m, true));
            }
            catch (Exception e)
            {
                _logger.LogError("Exception occured while publishing a message. Message: {Message}", e.Message);
            }

            await Task.Delay(10000, stoppingToken);
        }    
    }
}