using Crawler.UrlRepository.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Crawler.UrlRepository
{
    public static class DependencyInjection
    {
        public static IServiceCollection InjectDatabaseDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<UrlRepositoryContext>(options => options.UseSqlServer(connectionString));
            services.AddScoped<IRepository, Repository.UrlRepository>();
            
            return services;
        }
    }
}
