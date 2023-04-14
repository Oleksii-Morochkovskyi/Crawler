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
            services.AddWebAppDependencies();
            services.AddCrawlerDependencies();
            services.AddServiceDependencies();

            return services;
        }
    }
}
