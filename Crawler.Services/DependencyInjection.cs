
using Crawler.Logic.Interfaces;
using Crawler.Services.Helpers;
using Crawler.Services.Services;
using Crawler.Services.Wrappers;
using Microsoft.Extensions.DependencyInjection;

namespace Crawler.Services
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServiceDependencies(this IServiceCollection services)
        {
            services.AddScoped<DatabaseInteractionService>();
            services.AddScoped<MapModelsHelper>();
            services.AddScoped<IConsoleHandler, ConsoleWrapper>();

            return services;
        }
    }
}
