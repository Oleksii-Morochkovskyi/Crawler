using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Crawler.UrlRepository
{
    public class CrawlerDatabaseContextFactory : IDesignTimeDbContextFactory<CrawlerDatabaseContext>
    {
        public CrawlerDatabaseContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CrawlerDatabaseContext>();
            optionsBuilder.UseSqlServer("Server = localhost; Database = master; Trusted_Connection = True; TrustServerCertificate = true");

            return new CrawlerDatabaseContext(optionsBuilder.Options);
        }
    }
}
