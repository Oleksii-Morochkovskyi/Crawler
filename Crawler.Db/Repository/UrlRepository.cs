using Crawler.Logic.Models;
using Crawler.UrlRepository.Entities;

namespace Crawler.UrlRepository.Repository
{
    public class UrlRepository : IRepository
    {
        private readonly UrlRepositoryContext _dbContext;

        public UrlRepository(UrlRepositoryContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddFoundUrlsAsync(IEnumerable<UrlResponse> urls, string baseUrl)
        {
            var baseUrlId = _dbContext.InitialUrls.OrderBy(u => u.Id).Last().Id;

            var foundUrls = urls.Select(url => new FoundUrl { Url = url.Url, Location = url.Location, ResponseTimeMs = url.ResponseTimeMs, InitialUrlId = baseUrlId});

            _dbContext.FoundUrls.AddRange(foundUrls);

            await _dbContext.SaveChangesAsync();
        }

        public async Task AddInitialUrlAsync(string url)
        {
            var initialUrl = new InitialUrl { BaseUrl = url };

            _dbContext.InitialUrls.Add(initialUrl);

            await _dbContext.SaveChangesAsync();
        }
    }
}