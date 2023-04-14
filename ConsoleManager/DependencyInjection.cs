using Crawler.Logic.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Crawler.ConsoleOutput
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddConsoleDependencies(this IServiceCollection services)
        {
            services.AddScoped<ConsoleProcessor>();
            services.AddScoped<ConsoleOutput.DatabaseInteraction>();
            services.AddScoped<IConsoleHandler, ConsoleWrapper>();

            return services;
        }
    }
}
