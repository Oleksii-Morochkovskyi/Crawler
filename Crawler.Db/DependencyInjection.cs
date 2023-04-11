using Crawler.UrlRepository.EntityConfigurations;
using Crawler.UrlRepository.Interfaces;
using Crawler.UrlRepository.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Crawler.UrlRepository
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDatabaseDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<CrawlerDatabaseContext>(options => options.UseSqlServer(connectionString));
            services.AddScoped<IInitialUrlRepository, InitialUrlRepository>();
            services.AddScoped<IFoundUrlRepository, FoundUrlRepository>();
            //services.AddSingleton<FoundUrlConfiguration>();

            return services;
        }
    }
}
