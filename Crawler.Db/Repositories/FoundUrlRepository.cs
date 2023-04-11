using Crawler.Logic.Models;
using Crawler.UrlRepository.Entities;
using Crawler.UrlRepository.Interfaces;

namespace Crawler.UrlRepository.Repositories
{
    public class FoundUrlRepository : IFoundUrlRepository
    {
        private readonly CrawlerDatabaseContext _dbContext;

        public FoundUrlRepository(CrawlerDatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddFoundUrlsAsync(IEnumerable<UrlResponse> urls)
        {
            var baseUrlId = _dbContext.InitialUrls.Max(u => u.Id);
            
            var foundUrls = urls.Select(url => new FoundUrl
            {
                Url = url.Url, 
                Location = url.Location, 
                ResponseTimeMs = url.ResponseTimeMs, 
                InitialUrlId = baseUrlId
            });

            _dbContext.FoundUrls.AddRange(foundUrls);

            await _dbContext.SaveChangesAsync();
        }
    }
}
