using Direct.Infrastructure;
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
            app.SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        })
        .ConfigureServices((context, services) =>
        {
            CompositionRoot.AddDependencies(services, context.Configuration);
        });
}