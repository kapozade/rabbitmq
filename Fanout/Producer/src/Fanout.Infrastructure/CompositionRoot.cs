using System.Diagnostics;
using Fanout.Application.BackgroundServices;
using Fanout.Core.Messaging;
using Fanout.Core.Messaging.Settings;
using Fanout.Infrastructure.Messaging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fanout.Infrastructure;

public static class CompositionRoot
{
    public static void AddDependencies(IServiceCollection serviceCollection, IConfiguration configuration)
    {
        var rabbitMqSettings = configuration.GetSection("RabbitMqSettings").Get<RabbitMqSettings>()
                               ?? throw new UnreachableException("RabbitMqSettings is not configured properly.");

        serviceCollection.AddSingleton(rabbitMqSettings);
        serviceCollection.AddSingleton<IFakeDataQueueProducer, FakeDataQueueProducer>();
        serviceCollection.AddHostedService<FakeDataProducerBackgroundService>();
    }
}