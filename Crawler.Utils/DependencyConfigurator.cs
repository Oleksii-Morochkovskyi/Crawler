using Crawler.Utils.Mappers;
using Microsoft.Extensions.DependencyInjection;

namespace Crawler.Utils;

public static class DependencyConfigurator
{
    public static IServiceCollection AddUtilDependencies(this IServiceCollection services)
    {
        services.AddScoped<ModelMapper>();

        return services;
    }
}