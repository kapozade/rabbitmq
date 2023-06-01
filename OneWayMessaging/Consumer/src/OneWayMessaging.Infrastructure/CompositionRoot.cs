using Infrastructure.BackgroundServices;
using Infrastructure.Messaging;
using Infrastructure.Messaging.Core.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class CompositionRoot
{
    public static void AddDependencies(IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        var rabbitMqSettings = configuration.GetSection("RabbitMqSettings").Get<RabbitMqSettings>()
                               ?? throw new Exception("RabbitMqSettings is not configured properly!");
        serviceCollection.AddSingleton(rabbitMqSettings);

        serviceCollection.AddSingleton<IFakeDataQueue, FakeDataQueue>();
        serviceCollection.AddHostedService<FakeDataQueueConsumerHostedService>();
    }
}
