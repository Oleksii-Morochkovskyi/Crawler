using Crawler.Logic.Models;
using Microsoft.EntityFrameworkCore;

namespace Crawler.Db
{
    public class CrawlerDbContext : DbContext
    {
        public DbSet<UrlResponse> UrlResponse => Set<UrlResponse>();

        public CrawlerDbContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost;Database=master;Trusted_Connection=True;TrustServerCertificate=true");
        }
    }
}