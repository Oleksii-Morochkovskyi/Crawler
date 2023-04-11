using Crawler.UrlRepository.Entities;
using Crawler.UrlRepository.Interfaces;

namespace Crawler.UrlRepository.Repositories
{
    public class InitialUrlRepository : IInitialUrlRepository
    {
        private readonly UrlDatabaseContext _dbContext;

        public InitialUrlRepository(UrlDatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddInitialUrlAsync(string url)
        {
            var initialUrl = new InitialUrl { BaseUrl = url };

            _dbContext.InitialUrls.Add(initialUrl);

            await _dbContext.SaveChangesAsync();
        }
    }
}