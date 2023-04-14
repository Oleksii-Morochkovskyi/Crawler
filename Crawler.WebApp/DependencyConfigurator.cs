using Crawler.ConsoleOutput;
using Crawler.Logic;
using Crawler.Persistence;

namespace Crawler.WebApp
{
    public class DependencyConfigurator
    {
        public WebApplication ConfigureHost(IConfiguration configurator, WebApplicationBuilder builder)
        {
            builder.Host.ConfigureServices(services =>
            {
                services.AddDatabaseDependencies(configurator);
                services.AddConsoleDependencies();
                services.AddCrawlerDependencies();
            });
            var build = builder.Build();
            return build;
        }
    }
}
