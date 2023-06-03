using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Topic.Core.Data;
using Topic.Core.Messaging;

namespace Topic.Application;

public class Topic2DataProducerBackgroundService : IHostedService
{
    private readonly ITopic2DataProducer _topic2DataProducer;
    private readonly ILogger<Topic1DataProducerBackgroundService> _logger;

    public Topic2DataProducerBackgroundService(
            ITopic2DataProducer topic2DataProducer,
            ILogger<Topic1DataProducerBackgroundService> logger
        )
    {
        _topic2DataProducer = topic2DataProducer;
        _logger = logger;
    }
    
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                _topic2DataProducer.Publish(new FakeData("topic-2", 1, 12.2m, true));
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception occured while publishing a message. Message: {Message}", ex.Message);
            }
            await Task.Delay(2000, cancellationToken);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _topic2DataProducer.Dispose();
        return Task.CompletedTask;
    }
}