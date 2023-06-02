using DirectWithRouting.Core.Data;
using DirectWithRouting.Core.Messaging;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DirectWithRouting.Application;

public sealed class FakeDataProducerBackgroundService : IHostedService
{
    private readonly IFakeDataQueueProducer _fakeDataQueueProducer;
    private readonly ILogger<FakeDataProducerBackgroundService> _logger;

    public FakeDataProducerBackgroundService(
        IFakeDataQueueProducer fakeDataQueueProducer,
        ILogger<FakeDataProducerBackgroundService> logger
    )
    {
        _fakeDataQueueProducer = fakeDataQueueProducer;
        _logger = logger;
    }
    
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                _fakeDataQueueProducer.Publish(
                    new FakeData("Lorem İpsum", 1, 1.23m, true)
                );
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception occurred. Message: {Message}", ex.Message);
            }

            await Task.Delay(2000, cancellationToken);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _fakeDataQueueProducer.Dispose();
        return Task.CompletedTask;
    }
}