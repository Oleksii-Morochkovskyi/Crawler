using Crawler.Db.Repository;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Crawler.Db.DependencyInjection
{
    public class DatabaseDI
    {
        public IHostBuilder InjectDependencies(IHostBuilder builder)
        {
            builder.ConfigureServices(
                services =>
                {
                    services.AddScoped<IRepository, Repository.Repository>();
                });

            return builder;
        }
    }
}
