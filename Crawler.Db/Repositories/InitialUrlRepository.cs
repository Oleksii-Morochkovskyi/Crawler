using Crawler.Persistence.Entities;
using Crawler.Persistence.Interfaces;

namespace Crawler.Persistence.Repositories
{
    public class InitialUrlRepository : IInitialUrlRepository
    {
        private readonly UrlDatabaseContext _dbContext;

        public InitialUrlRepository(UrlDatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<InitialUrl> AddInitialUrlAsync(string url)
        {
            var initialUrl = new InitialUrl { BaseUrl = url };

            _dbContext.InitialUrls.Add(initialUrl);

            await _dbContext.SaveChangesAsync();

            return initialUrl;
        }
    }
}