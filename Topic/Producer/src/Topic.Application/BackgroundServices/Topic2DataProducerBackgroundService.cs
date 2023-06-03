using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Topic.Core.Data;
using Topic.Core.Messaging;

namespace Topic.Application.BackgroundServices;

public sealed class Topic2DataProducerBackgroundService : IHostedService
{
    private readonly ITopic2DataProducerQueue _topic2DataProducerQueue;
    private readonly ILogger<Topic2DataProducerBackgroundService> _logger;

    public Topic2DataProducerBackgroundService(
            ITopic2DataProducerQueue topic2DataProducerQueue,
            ILogger<Topic2DataProducerBackgroundService> logger
            )
    {
        _topic2DataProducerQueue = topic2DataProducerQueue;
        _logger = logger;
    }
    
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("Topic 2 background service is starting");
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                _topic2DataProducerQueue.Publish(new FakeData("topic-2", 2, 2.23m, false));
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
        _topic2DataProducerQueue.Dispose();
        return Task.CompletedTask;
    }
}