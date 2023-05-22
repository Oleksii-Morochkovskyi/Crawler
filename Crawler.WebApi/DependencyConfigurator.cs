using Crawler.Logic;
using Crawler.Persistence;
using Crawler.Utils;

namespace Crawler.WebApi
{
    public static class DependencyConfigurator
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configurator)
        {
            services.AddDatabaseDependencies(configurator);
            services.AddCrawlerDependencies();
            services.AddServiceDependencies();

            return services;
        }
    }
}
