using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Topic.Core.Data;
using Topic.Core.Messaging;

namespace Topic.Application;

public class DataProducersBackgroundService : IHostedService
{
    private readonly ITopic1DataProducer _topic1DataProducer;
    private readonly ITopic1SubCategoryDataProducer _topic1SubCategoryDataProducer;
    private readonly ITopic2DataProducer _topic2DataProducer;
    private readonly ILogger<DataProducersBackgroundService> _logger;

    public DataProducersBackgroundService(
            IServiceProvider serviceProvider
        )
    {
        using var scope = serviceProvider.CreateScope();
        _topic1DataProducer = scope.ServiceProvider.GetRequiredService<ITopic1DataProducer>();
        _topic2DataProducer = scope.ServiceProvider.GetRequiredService<ITopic2DataProducer>();
        _topic1SubCategoryDataProducer = scope.ServiceProvider.GetRequiredService<ITopic1SubCategoryDataProducer>();
        _logger = scope.ServiceProvider.GetRequiredService<ILogger<DataProducersBackgroundService>>();
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                _topic1DataProducer.Publish(new FakeData("topic1", 1, 1.12m, true));
                _topic2DataProducer.Publish(new FakeData("topic2", 1, 1.12m, true));
                _topic1SubCategoryDataProducer.Publish(new FakeData("topic1-sub", 1, 1.12m, true));
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