using Crawler.ConsoleOutput;
using Crawler.Persistence;
using Crawler.Logic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Graph.Models;
using WebApplication = Microsoft.Graph.Models.WebApplication;

namespace Crawler.WebApp
{
    public class DependencyConfigurator
    {
        public Microsoft.AspNetCore.Builder.WebApplication ConfigureHost(IConfiguration configurator, WebApplicationBuilder builder)
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
