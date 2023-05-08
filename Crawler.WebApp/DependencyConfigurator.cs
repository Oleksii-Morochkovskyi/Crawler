using Crawler.ConsoleOutput;
using Crawler.Logic;
using Crawler.Persistence;
using Crawler.Services;

namespace Crawler.WebApp
{
    public static class DependencyConfigurator
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configurator)
        {
            services.AddDatabaseDependencies(configurator);
            services.AddConsoleDependencies();
            services.AddCrawlerDependencies();
            services.AddServiceDependencies();
            services.AddWebAppDependencies();

            return services;
        }
    }
}
