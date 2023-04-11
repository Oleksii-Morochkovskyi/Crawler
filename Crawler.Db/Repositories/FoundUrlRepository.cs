using Crawler.Logic.Models;
using Crawler.Persistence.Entities;
using Crawler.Persistence.Interfaces;

namespace Crawler.Persistence.Repositories
{
    public class FoundUrlRepository : IFoundUrlRepository
    {
        private readonly UrlDatabaseContext _dbContext;

        public FoundUrlRepository(UrlDatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddFoundUrlsAsync(InitialUrl initialUrl, IEnumerable<UrlResponse> urls)
        {
            var foundUrls = urls.Select(url => new FoundUrl
            {
                Url = url.Url, 
                Location = url.Location, 
                ResponseTimeMs = url.ResponseTimeMs,
                InitialUrl = initialUrl,
                InitialUrlId = initialUrl.Id
            });

            _dbContext.FoundUrls.AddRange(foundUrls);

            await _dbContext.SaveChangesAsync();
        }
    }
}
