using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WorkQueue.Core.Data;
using WorkQueue.Core.Messaging;

namespace WorkerQueue.Application.BackgroundServices;

public class FakeDataProducerBackgroundService : IHostedService
{
    private readonly IFakeDataProducer _fakeDataProducer;
    private readonly ILogger<FakeDataProducerBackgroundService> _logger;

    public FakeDataProducerBackgroundService(
            IFakeDataProducer fakeDataProducer,
            ILogger<FakeDataProducerBackgroundService> logger
        )
    {
        _fakeDataProducer = fakeDataProducer;
        _logger = logger;
    }
    
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                _fakeDataProducer.Publish(new FakeData
                {
                    Field1 = "Lorem İpsum",
                    Field2 = 1,
                    Field3 = 1.23m,
                    Field4 = true
                });
                _logger.LogInformation("Message sent at: {Date}", DateTime.Now.ToString("O"));
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
        _fakeDataProducer.Dispose();
        return Task.CompletedTask;
    }
}