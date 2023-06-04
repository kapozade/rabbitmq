using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Topic.Core.Data;
using Topic.Core.Messaging;

namespace Topic.Application.BackgroundServices;

public sealed class Topic1SubCategoryDataProducerBackgroundService : BackgroundService
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
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                _dataProducer.Publish(new FakeData("topic1-sub", 1, 1.23m, true));
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception occured while publishing a message. Message: {Message}", ex.Message);
            }

            await Task.Delay(10000, stoppingToken);
        }
    }
}