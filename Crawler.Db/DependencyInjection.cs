using Crawler.Persistence.Interfaces;
using Crawler.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Crawler.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDatabaseDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<UrlDatabaseContext>(options => options.UseSqlServer(connectionString));
            services.AddScoped<IInitialUrlRepository, InitialUrlRepository>();
            services.AddScoped<IFoundUrlRepository, FoundUrlRepository>();

            return services;
        }
    }
}
