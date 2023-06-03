using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Topic.Application.BackgroundServices;
using Topic.Core.Messaging;
using Topic.Core.Messaging.Settings;
using Topic.Infrastructure.Messaging;

namespace Topic.Infrastructure;

public static class CompositionRoot
{
    public static void AddDependencies(IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        var rabbitMqSettings = configuration.GetSection("RabbitMqSettings").Get<RabbitMqSettings>()
                               ?? throw new UnreachableException("RabbitMqSettings is not configured properly.");
        serviceCollection.AddSingleton(rabbitMqSettings);

        serviceCollection.AddSingleton<ITopic1DataConsumerQueue, Topic1DataConsumerQueue>();
        serviceCollection.AddSingleton<ITopic2DataConsumerQueue, Topic2DataConsumerQueue>();

        serviceCollection.AddHostedService<Topic1DataConsumerBackgroundService>();
        serviceCollection.AddHostedService<Topic2DataConsumerBackgroundService>();
    }
}
