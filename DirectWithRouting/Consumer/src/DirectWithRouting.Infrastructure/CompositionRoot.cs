using System.Diagnostics;
using DirectWithRouting.Application.BackgroundService;
using DirectWithRouting.Core.Messaging;
using DirectWithRouting.Core.Messaging.Settings;
using DirectWithRouting.Infrastructure.Messaging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DirectWithRouting.Infrastructure;

public static class CompositionRoot
{
    public static void AddDependencies(IServiceCollection serviceCollection, IConfiguration configuration)
    {
        var rabbitMqSettings = configuration.GetSection("RabbitMqSettings").Get<RabbitMqSettings>()
                               ?? throw new UnreachableException("RabbitMqSettings is not configured properly.");

        serviceCollection.AddSingleton(rabbitMqSettings);
        serviceCollection.AddSingleton<IFakeDataQueueConsumer, FakeDataQueueQueueConsumer>();
        serviceCollection.AddHostedService<FakeDataConsumerBackgroundService>();
    }
}