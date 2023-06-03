using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Topic.Core.Data;
using Topic.Core.Messaging;

namespace Topic.Application;

public class Topic1DataProducerBackgroundService : IHostedService
{
    private readonly ITopic1DataProducer _topic1DataProducer;
    private readonly ILogger<Topic1DataProducerBackgroundService> _logger;

    public Topic1DataProducerBackgroundService(
            ITopic1DataProducer topic1DataProducer,
            ILogger<Topic1DataProducerBackgroundService> logger
        )
    {
        _topic1DataProducer = topic1DataProducer;
        _logger = logger;
    }
    
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("Topic1 BS is starting");
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                _topic1DataProducer.Publish(new FakeData("topic-1", 1, 1.12m, true));
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
        _topic1DataProducer.Dispose();
        return Task.CompletedTask;
    }
}