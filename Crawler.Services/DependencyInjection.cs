using Crawler.Logic.Interfaces;
using Crawler.Services.Helpers;
using Crawler.Services.Wrappers;
using Crawler.Utils.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Crawler.Utils
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
