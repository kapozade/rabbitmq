using Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

var hostBuilder = CreateBuilder();
var host = hostBuilder.Build();
host.Run();

static IHostBuilder CreateBuilder()
{
    return Host.CreateDefaultBuilder()
        .ConfigureAppConfiguration(app =>
        { 
            app.AddJsonFile("appsettings.json");
        })
        .ConfigureServices((context, services) =>
        {
            CompositionRoot.AddDependencies(services, context.Configuration);
        });
}