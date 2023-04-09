using Crawler.Db.Entities;
using Crawler.Logic.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Crawler.Db.Repository
{
    public class Repository : IRepository
    {
        private readonly CrawlerDbContext _dbContext;

        public Repository(/*IConfiguration configuration*/CrawlerDbContext dbContext)
        {
           // var options = ConfigureRepository(configuration);
           _dbContext = dbContext;
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

        /*public DbContextOptions<CrawlerDbContext> ConfigureRepository(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            var optionsBuilder = new DbContextOptionsBuilder<CrawlerDbContext>();

            return optionsBuilder.UseSqlServer(connectionString).Options;
        }*/
    }
}