using Crawler.InfrastructureIoC;
using Crawler.Utils;

namespace Crawler.WebApi
{
    public static class DependencyConfigurator
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configurator)
        {
            services.AddDatabaseDependencies(configurator);
            services.AddApplicationDependencies();
            services.AddUtilDependencies();
            
            return services;
        }
    }
}
