using Crawler.ConsoleOutput.Wrappers;
using Microsoft.Extensions.DependencyInjection;

namespace Crawler.ConsoleOutput
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddConsoleDependencies(this IServiceCollection services)
        {
            services.AddScoped<ConsoleProcessor>();
            services.AddScoped<DatabaseInteraction>();
            services.AddScoped<ConsoleWrapper>();
            
            return services;
        }
    }
}
