using Crawler.WebApp.Models;

namespace Crawler.WebApp
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddWebAppDependencies(this IServiceCollection services)
        {
            services.AddScoped<FoundUrlViewModel>();
            services.AddScoped<InitialUrlViewModel>();
            services.AddScoped<ResultViewModel>();

            return services;
        }
    }
}
