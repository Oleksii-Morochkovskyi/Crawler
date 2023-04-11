using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Crawler.Persistence
{
    public class UrlDatabaseContextFactory : IDesignTimeDbContextFactory<UrlDatabaseContext>
    {
        public UrlDatabaseContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<UrlDatabaseContext>();
            optionsBuilder.UseSqlServer("Server = localhost; Database = master; Trusted_Connection = True; TrustServerCertificate = true");

            return new UrlDatabaseContext(optionsBuilder.Options);
        }
    }
}
