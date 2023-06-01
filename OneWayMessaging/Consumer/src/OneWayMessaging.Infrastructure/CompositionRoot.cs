using System.Diagnostics;
using Infrastructure.Messaging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OneWayMessaging.Application.BackgroundService;
using OneWayMessaging.Core.Messaging;
using OneWayMessaging.Core.Messaging.Settings;

namespace Infrastructure;

public static class CompositionRoot
{
    public static void AddDependencies(IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        var rabbitMqSettings = configuration.GetSection("RabbitMqSettings").Get<RabbitMqSettings>()
                               ?? throw new UnreachableException("RabbitMqSettings is not configured properly!");
        serviceCollection.AddSingleton(rabbitMqSettings);

        serviceCollection.AddSingleton<IFakeDataQueue, FakeDataQueue>();
        serviceCollection.AddHostedService<FakeDataQueueConsumerHostedService>();
    }
}
