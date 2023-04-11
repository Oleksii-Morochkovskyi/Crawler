using Crawler.Logic.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Crawler.ConsoleOutput
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddConsoleDependencies(this IServiceCollection services)
        {
            services.AddSingleton<ConsoleProcessor>();
            services.AddSingleton<DatabaseInteraction>();
            services.AddScoped<IConsoleHandler, ConsoleWrapper>();

            return services;
        }
    }
}
