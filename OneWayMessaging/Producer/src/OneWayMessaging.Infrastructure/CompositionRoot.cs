using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OneWayMessaging.Application.BackgroundService;
using OneWayMessaging.Core.Messaging;
using OneWayMessaging.Core.Messaging.Settings;
using OneWayMessaging.Infrastructure.Messaging;

namespace OneWayMessaging.Infrastructure;

public static class CompositionRoot
{
    public static void AddDependencies(IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        var rabbitMqSettings = configuration.GetSection("RabbitMqSettings").Get<RabbitMqSettings>()
                               ?? throw new UnreachableException("RabbitMqSettings is not configured properly!");
        serviceCollection.AddSingleton(rabbitMqSettings);

        serviceCollection.AddSingleton<IFakeDataQueueProducer, FakeDataQueueProducer>();
        serviceCollection.AddHostedService<FakeDataProducerHostedService>();
    }
}