using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Topic.Core.Data;
using Topic.Core.Messaging;

namespace Topic.Application;

public class Topic1SubCategoryDataProducerBackgroundService : IHostedService
{
    private readonly ITopic1SubCategoryDataProducer _dataProducer;
    private readonly ILogger<Topic1SubCategoryDataProducerBackgroundService> _logger;

    public Topic1SubCategoryDataProducerBackgroundService(
            ITopic1SubCategoryDataProducer dataProducer,
            ILogger<Topic1SubCategoryDataProducerBackgroundService> logger
        )
    {
        _dataProducer = dataProducer;
        _logger = logger;
    }
    
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                _dataProducer.Publish(new FakeData("topic-1-sub", 2, 2.3m, true));
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
        _dataProducer.Dispose();
        return Task.CompletedTask;
    }
}