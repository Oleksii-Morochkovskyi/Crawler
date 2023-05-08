using Crawler.ConsoleOutput;
using Crawler.Logic;
using Crawler.Persistence;
using Crawler.Services;

namespace Crawler.WebApi
{
    public static class DependencyConfigurator
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configurator)
        {
            services.AddDatabaseDependencies(configurator);
            services.AddConsoleDependencies();
            services.AddWebApiDependencies();
            services.AddCrawlerDependencies();
            services.AddServiceDependencies();

            return services;
        }
    }
}
