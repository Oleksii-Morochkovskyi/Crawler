using Crawler.WebApi.Models;

namespace Crawler.WebApi
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddWebApiDependencies(this IServiceCollection services)
        {
            services.AddScoped<FoundUrlViewModel>();
            services.AddScoped<InitialUrlViewModel>();
            services.AddScoped<ResultViewModel>();

            return services;
        }
    }
}
