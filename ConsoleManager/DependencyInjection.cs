using Crawler.Logic.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Crawler.ConsoleOutput
{
    public static class DependencyInjection
    {
        public static IServiceCollection InjectConsoleDependencies(this IServiceCollection services)
        {
            services.AddSingleton<ConsoleProcessor>();
            services.AddScoped<IConsoleHandler, ConsoleWrapper>();
            
            return services;
        }
    }
}
