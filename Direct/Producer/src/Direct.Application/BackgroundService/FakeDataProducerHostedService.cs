using Direct.Core.Data;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Direct.Core.Messaging;

namespace Direct.Application.BackgroundService;

public sealed class FakeDataProducerHostedService : IHostedService
{
    private readonly IFakeDataQueueProducer _fakeDataQueueProducer;
    private readonly ILogger<FakeDataProducerHostedService> _logger;

    public FakeDataProducerHostedService(
        IFakeDataQueueProducer fakeDataQueueProducer,
        ILogger<FakeDataProducerHostedService> logger
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
                var fakeData = new FakeData
                {
                    Field1 = "Lorem İpsum Dolor",
                    Field2 = "Lorem ipsum Dolor",
                    Field3 = 1.123m,
                    Field4 = 43,
                    Field5 = true
                };
            
                _fakeDataQueueProducer.Publish(fakeData);
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