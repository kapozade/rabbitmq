using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Topic.Core.Data;
using Topic.Core.Messaging;

namespace Topic.Application.BackgroundServices;

public sealed class Topic2DataProducerBackgroundService : BackgroundService
{
    private readonly ITopic2DataProducer _dataProducer;
    private readonly ILogger<Topic1SubCategoryDataProducerBackgroundService> _logger;

    public Topic2DataProducerBackgroundService(
            ITopic2DataProducer dataProducer,
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
                _dataProducer.Publish(new FakeData("topic2", 2, 2.23m, true));
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception occured while publishing a message. Message: {Message}", ex.Message);
            }
            
            await Task.Delay(10000, stoppingToken);
        }
    }
}