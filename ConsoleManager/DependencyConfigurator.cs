using Crawler.ConsoleOutput;
using Crawler.InfrastructureIoC;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Crawler.ConsoleOutput
{
    public class DependencyConfigurator
    {
        public IHostBuilder ConfigureHost(IConfiguration configurator)
        {
            var builder = Host.CreateDefaultBuilder();

            builder.ConfigureLogging(logging => logging.ClearProviders())
                .ConfigureServices(services =>
                {
                    services.AddDatabaseDependencies(configurator);
                    services.AddApplicationDependencies();
                    services.AddConsoleDependencies();

                });

            return builder;
        }
    }
}
