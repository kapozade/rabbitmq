using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Topic.Core.Messaging.Settings;

namespace Topic.Infrastructure;

public static class CompositionRoot
{
    public static void AddDependencies(IServiceCollection serviceCollection, IConfiguration configuration)
    {
        var rabbitMqSettings = configuration.GetSection("RabbitMqSettings").Get<RabbitMqSettings>()
                               ?? throw new UnreachableException("RabbitMqSettings is not configured properly");

        serviceCollection.AddSingleton(rabbitMqSettings);
    }
}