using Crawler.Db.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Crawler.Db.DependencyInjection
{
    public class DatabaseDI
    {
        public IHostBuilder InjectDependencies(IHostBuilder builder)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");
            
            builder.ConfigureServices(
                services =>
                {
                    services.AddScoped<IRepository, Repository.Repository>();
                    services.AddDbContext<CrawlerDbContext>(options => options.UseSqlServer(connectionString));
                });

            return builder;
        }
    }
}
