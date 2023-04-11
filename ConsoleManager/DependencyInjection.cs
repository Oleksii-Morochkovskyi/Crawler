using Crawler.Logic.Interfaces;
using Crawler.UrlRepository.Repositories;
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
            services.AddSingleton<InitialUrlRepository>();
            services.AddSingleton<FoundUrlRepository>();

            return services;
        }
    }
}
