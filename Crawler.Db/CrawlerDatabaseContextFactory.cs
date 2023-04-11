using Crawler.UrlRepository.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Crawler.UrlRepository
{
    public class CrawlerDatabaseContextFactory : IDesignTimeDbContextFactory<CrawlerDatabaseContext>
    {
        private readonly FoundUrlConfiguration _foundUrlConfiguration;

        public CrawlerDatabaseContextFactory(FoundUrlConfiguration foundUrlConfiguration)
        {
            _foundUrlConfiguration = foundUrlConfiguration;
        }

        public CrawlerDatabaseContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CrawlerDatabaseContext>();
            optionsBuilder.UseSqlServer("Server = localhost; Database = master; Trusted_Connection = True; TrustServerCertificate = true");

            return new CrawlerDatabaseContext(optionsBuilder.Options);
        }
    }
}
