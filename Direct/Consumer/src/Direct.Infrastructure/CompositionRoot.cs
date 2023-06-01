using System.Diagnostics;
using Direct.Infrastructure.Messaging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Direct.Application.BackgroundService;
using Direct.Core.Messaging;
using Direct.Core.Messaging.Settings;

namespace Direct.Infrastructure;

public static class CompositionRoot
{
    public static void AddDependencies(IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        var rabbitMqSettings = configuration.GetSection("RabbitMqSettings").Get<RabbitMqSettings>()
                               ?? throw new UnreachableException("RabbitMqSettings is not configured properly!");
        serviceCollection.AddSingleton(rabbitMqSettings);

        serviceCollection.AddSingleton<IFakeDataQueueConsumer, FakeDataQueueConsumer>();
        serviceCollection.AddHostedService<FakeDataQueueConsumerHostedService>();
    }
}
