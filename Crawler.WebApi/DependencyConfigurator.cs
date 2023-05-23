using Crawler.InfrastructureIoC;

namespace Crawler.WebApi
{
    public static class DependencyConfigurator
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configurator)
        {
            services.AddDatabaseDependencies(configurator);
            services.AddApplicationDependencies();

            return services;
        }
    }
}
