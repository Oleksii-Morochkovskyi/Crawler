using Crawler.Db.Entities;
using Crawler.Logic.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Crawler.Db.Repository
{
    public class Repository : IRepository
    {
        private readonly CrawlerDbContext _dbContext;

        public Repository()
        {
            var options = ConfigureRepository();
            _dbContext = new CrawlerDbContext(options);
        }

        public void AddFoundUrls(IEnumerable<UrlResponse> urls, string baseUrl)
        {
            var foundUrls = FillFoundUrls(urls, baseUrl);

            _dbContext.FoundUrls.AddRange(foundUrls);
        }

        public async Task SaveChanges()
        {
            await _dbContext.SaveChangesAsync();
        }

        private IEnumerable<FoundUrl> FillFoundUrls(IEnumerable<UrlResponse> urls, string baseUrl)
        {
            var domainUrl = new DomainUrl { BaseUrl = baseUrl };

            return urls.Select(url => new FoundUrl { Url = url.Url, Location = url.Location, ResponseTimeMs = url.ResponseTimeMs, DomainUrl = domainUrl });
        }

        public DbContextOptions<CrawlerDbContext> ConfigureRepository()
        {
            var builder = new ConfigurationBuilder();

            builder.SetBasePath(Directory.GetCurrentDirectory());

            builder.AddJsonFile("appsettings.json");

            var config = builder.Build();

            var connectionString = config.GetConnectionString("DefaultConnection");

            var optionsBuilder = new DbContextOptionsBuilder<CrawlerDbContext>();

            return optionsBuilder.UseSqlServer(connectionString).Options;
        }
    }
}