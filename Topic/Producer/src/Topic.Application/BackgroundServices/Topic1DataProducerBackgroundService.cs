using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Topic.Core.Data;
using Topic.Core.Messaging;

namespace Topic.Application.BackgroundServices;

public sealed class Topic1DataProducerBackgroundService : IHostedService
{
    private readonly ITopic1DataProducerQueue _topic1DataProducerQueue;
    private readonly ILogger<Topic1DataProducerBackgroundService> _logger;

    public Topic1DataProducerBackgroundService(
            ITopic1DataProducerQueue topic1DataProducerQueue,
            ILogger<Topic1DataProducerBackgroundService> logger
            )
    {
        _topic1DataProducerQueue = topic1DataProducerQueue;
        _logger = logger;
    }
    
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("Topic 1 background service is starting");
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                _topic1DataProducerQueue.Publish(new FakeData("topic-1", 1, 1.23m, true));
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
        _topic1DataProducerQueue.Dispose();
        return Task.CompletedTask;
    }
}