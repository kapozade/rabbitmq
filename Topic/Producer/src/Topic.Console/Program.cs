using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Topic.Infrastructure;

var hostBuilder = CreateHostBuilder();
var host = hostBuilder.Build();
host.Run();

static IHostBuilder CreateHostBuilder()
{
    return Host.CreateDefaultBuilder()
        .ConfigureAppConfiguration(cfg =>
        {
            cfg.SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        })
        .ConfigureServices((ctx, collection) =>
        {
            CompositionRoot.AddDependencies(collection, ctx.Configuration);
        });
}