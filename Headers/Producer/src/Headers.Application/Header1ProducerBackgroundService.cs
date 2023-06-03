using Headers.Core.Data;
using Headers.Core.Messaging;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Headers.Application;

public sealed class Header1ProducerBackgroundService : IHostedService
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
    
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                _producerQueue.Publish(new FakeData("Header 1", 1, 1.23m, true));
            }
            catch (Exception e)
            {
                _logger.LogError("Exception occured while publishing a message. Message: {Message}", e.Message);
            }

            await Task.Delay(2000, cancellationToken);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _producerQueue.Dispose();
        return Task.CompletedTask;
    }
}