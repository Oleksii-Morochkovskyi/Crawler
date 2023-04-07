using Crawler.Db.Enteties;
using Microsoft.EntityFrameworkCore;

namespace Crawler.Db
{
    public class CrawlerDbContext : DbContext
    {
        public DbSet<FoundUrl> FoundUrls { get; set; }
        public DbSet<DomainUrl> DomainUrls { get; set; }

        public CrawlerDbContext(DbContextOptions<CrawlerDbContext> options):base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FoundUrl>()
                .HasOne(u => u.DomainUrl)
                .WithMany()
                .HasForeignKey(u => u.DomainUrlId);
        }
    }
}