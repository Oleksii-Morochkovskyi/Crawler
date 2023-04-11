using Crawler.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Crawler.Persistence
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
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(UrlDatabaseContext).Assembly);
        }
    }
}