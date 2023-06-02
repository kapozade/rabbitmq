using DirectWithRouting.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

var hostBuilder = CreateDefaultBuilder();
var host = hostBuilder.Build();
host.Run();

static IHostBuilder CreateDefaultBuilder()
{
    return Host.CreateDefaultBuilder()
        .ConfigureAppConfiguration(cfg =>
        {
            cfg.SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        })
        .ConfigureServices((context, serviceCollection) =>
        {
            CompositionRoot.AddDependencies(serviceCollection, context.Configuration);
        });
}