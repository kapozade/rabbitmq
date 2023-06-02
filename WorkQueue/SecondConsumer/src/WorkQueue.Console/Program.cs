using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using WorkQueue.Infrastructure;

var hostBuilder = CreateDefaultBuilder();
var host = hostBuilder.Build();
host.Run();

static IHostBuilder CreateDefaultBuilder()
{
    return Host.CreateDefaultBuilder()
        .ConfigureAppConfiguration(app =>
        {
            app.SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        }).ConfigureServices((context, serviceCollection) =>
        {
            CompositionRoot.AddDependencies(serviceCollection, context.Configuration);
        });
}