using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Topic.Core.Data;
using Topic.Core.Messaging;

namespace Topic.Application.BackgroundServices;

public class Topic1DataProducerBackgroundService : BackgroundService
{
    private readonly ITopic1DataProducer _dataProducer;
    private readonly ILogger<Topic1DataProducerBackgroundService> _logger;

    public Topic1DataProducerBackgroundService(
            ITopic1DataProducer dataProducer,
            ILogger<Topic1DataProducerBackgroundService> logger
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
                _dataProducer.Publish(new FakeData("topic1", 1, 1.12m, true));
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception occured while publishing a message. Message: {Message}", ex.Message);
            }
            await Task.Delay(10000, stoppingToken);
        }
    }
}