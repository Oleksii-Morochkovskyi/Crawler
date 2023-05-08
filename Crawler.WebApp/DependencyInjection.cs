
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crawler.WebApp.Helpers;

namespace Crawler.WebApp
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddWebAppDependencies(this IServiceCollection services)
        {
            services.AddScoped<MapModelsHelper>();

            return services;
        }
    }
}
