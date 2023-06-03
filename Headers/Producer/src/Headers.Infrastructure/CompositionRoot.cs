using System.Diagnostics;
using Headers.Application;
using Headers.Core.Messaging;
using Headers.Core.Messaging.Settings;
using Headers.Infrastructure.Messaging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Headers.Infrastructure;

public static class CompositionRoot
{
    public static void AddDependencies(IServiceCollection serviceCollection, IConfiguration configuration)
    {
        var rabbitMqSettings = configuration.GetSection("RabbitMqSettings").Get<RabbitMqSettings>()
                               ?? throw new UnreachableException("RabbitMqSettings is not configured properly.");
        serviceCollection.AddSingleton(rabbitMqSettings);

        serviceCollection.AddSingleton<IHeader1ProducerQueue, Header1ProducerQueue>();
        serviceCollection.AddSingleton<IHeader2ProducerQueue, Header2ProducerQueue>();
        serviceCollection.AddSingleton<IHeader3ProducerQueue, Header3ProducerQueue>();

        serviceCollection.AddHostedService<Header1ProducerBackgroundService>();
        serviceCollection.AddHostedService<Header2ProducerBackgroundService>();
        serviceCollection.AddHostedService<Header3ProducerBackgroundService>();
    }
}