using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Crawler.UrlRepository
{
    public class UrlRepositoryContextFactory : IDesignTimeDbContextFactory<UrlRepositoryContext>
    {
        public UrlRepositoryContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<UrlRepositoryContext>();
            optionsBuilder.UseSqlServer("Server = localhost; Database = master; Trusted_Connection = True; TrustServerCertificate = true");

            return new UrlRepositoryContext(optionsBuilder.Options);
        }
    }
}
