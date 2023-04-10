using Crawler.UrlDatabase.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Crawler.UrlDatabase
{
    public static class DependencyInjection
    {
        public static IServiceCollection InjectDatabaseDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<UrlDatabaseContext>(options => options.UseSqlServer(connectionString));
            services.AddScoped<IRepository, UrlRepository>();
            
            return services;
        }
    }
}
