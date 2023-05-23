using Crawler.Application.Interfaces;
using Crawler.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Crawler.Persistence.Repositories
{
    public class FoundUrlRepository : IFoundUrlRepository
    {
        private readonly UrlDatabaseContext _dbContext;

        public FoundUrlRepository(UrlDatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> AddFoundUrlsAsync(InitialUrl initialUrl, IEnumerable<UrlResponse> urls)
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

            return initialUrl.Id;
        }

        public async Task<IEnumerable<FoundUrl>> GetUrlsByInitialUrlIdAsync(int id)
        {
            return await _dbContext.FoundUrls.Where(x=>x.InitialUrlId == id).ToListAsync();
        }
    }
}
