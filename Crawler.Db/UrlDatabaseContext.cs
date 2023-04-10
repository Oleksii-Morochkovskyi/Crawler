using Crawler.UrlDatabase.Entities;
using Microsoft.EntityFrameworkCore;

namespace Crawler.UrlDatabase
{
    public class UrlDatabaseContext : DbContext
    {
        public DbSet<FoundUrl> FoundUrls { get; set; }
        public DbSet<InitialUrl> InitialUrls { get; set; }

        public UrlDatabaseContext(DbContextOptions<UrlDatabaseContext> options):base(options)
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