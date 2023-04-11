using Crawler.UrlRepository.Entities;
using Microsoft.EntityFrameworkCore;

namespace Crawler.UrlRepository
{
    public class UrlRepositoryContext : DbContext
    {
        public DbSet<FoundUrl> FoundUrls { get; set; }
        public DbSet<InitialUrl> InitialUrls { get; set; }

        public UrlRepositoryContext(DbContextOptions<UrlRepositoryContext> options):base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FoundUrl>()
                .HasOne(u => u.InitialUrl)
                .WithMany(x => x.FoundUrls)
                .HasForeignKey(u => u.InitialUrlId);
        }
    }
}