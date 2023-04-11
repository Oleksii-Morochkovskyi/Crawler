using Crawler.UrlRepository;
using Crawler.Logic;
using Microsoft.Extensions.Configuration;
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
                services.InjectConsoleDependencies();
                services.InjectCrawlerDependencies();
                services.InjectDatabaseDependencies(configurator);
            });

            return builder;
        }
    }
}
