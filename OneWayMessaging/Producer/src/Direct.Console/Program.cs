using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Direct.Infrastructure;

var hostBuilder = CreateDefaultBuilder();
var host = hostBuilder.Build();
host.Run();

static IHostBuilder CreateDefaultBuilder()
{
    return Host.CreateDefaultBuilder()
        .ConfigureAppConfiguration(app =>
        {
            app.AddJsonFile("appsettings.json");
        })
        .ConfigureServices((context, serviceCollection) =>
        {
            CompositionRoot.AddDependencies(serviceCollection, context.Configuration);
        });
}