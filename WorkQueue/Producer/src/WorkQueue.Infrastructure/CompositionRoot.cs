using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WorkerQueue.Application.BackgroundServices;
using WorkQueue.Core.Messaging;
using WorkQueue.Core.Messaging.Settings;
using WorkQueue.Infrastructure.Messaging;

namespace WorkQueue.Infrastructure;

public static class CompositionRoot
{
    public static void AddDependencies(IServiceCollection serviceCollection, 
        IConfiguration configuration)
    {
        var rabbitMqSettings = configuration.GetSection("RabbitMqSettings")
                                   .Get<RabbitMqSettings>()
                               ?? throw new UnreachableException("RabbitMqSettings is not configured properly.");

        serviceCollection.AddSingleton(rabbitMqSettings);
        serviceCollection.AddSingleton<IFakeDataProducer, FakeDataProducer>();
        serviceCollection.AddHostedService<FakeDataProducerBackgroundService>();
    }
}