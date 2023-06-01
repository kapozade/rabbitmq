using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OneWayMessaging.Core.Messaging.Settings;

namespace OneWayMessaging.Infrastructure;

public static class CompositionRoot
{
    public static void AddDependencies(IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        var rabbitMqSettings = configuration.GetSection("RabbitMqSettings").Get<RabbitMqSettings>()
            ?? throw new UnreachableException("RabbitMqSettings is not configured properly!")
    }
}