using System.Diagnostics;
using Headers.Core.Messaging;
using Headers.Core.Messaging.Settings;
using Headers.Infrastructure.Messaging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Headers.Infrastructure;

public static class CompositionRoot
{
    public static void AddDependencies(IServiceCollection serviceCollection, 
        IConfiguration configuration)
    {
        var rabbitMqSettings = configuration.GetSection("RabbitMqSettings").Get<RabbitMqSettings>()
                               ?? throw new UnreachableException("RabbitMqSettings is not configured properly.");
        serviceCollection.AddSingleton(rabbitMqSettings);

        serviceCollection.AddSingleton<IHeader1ConsumerQueue, Header1ConsumerQueue>();
        serviceCollection.AddSingleton<IHeader2ConsumerQueue, Header2ConsumerQueue>();
    }
}