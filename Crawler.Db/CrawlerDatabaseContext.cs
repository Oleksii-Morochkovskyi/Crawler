using Crawler.UrlRepository.Entities;
using Crawler.UrlRepository.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

namespace Crawler.UrlRepository
{
    public class CrawlerDatabaseContext : DbContext
    {
        public DbSet<FoundUrl> FoundUrls { get; set; }
        public DbSet<InitialUrl> InitialUrls { get; set; }

        public CrawlerDatabaseContext(DbContextOptions<CrawlerDatabaseContext> options):base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new FoundUrlConfiguration());
        }
    }
}