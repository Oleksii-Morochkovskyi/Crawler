using Crawler.Db;
using Crawler.Logic.Models;

namespace Crawler.Repository
{
    public class DbHandler : IRepository
    {
        private readonly CrawlerDbContext _dbContext;

        public DbHandler()
        {
            _dbContext = new CrawlerDbContext();
        }
        public void AddElements(IEnumerable<UrlResponse> urls)
        {
            _dbContext.UrlResponse.AddRange(urls);
        }

        public async Task SaveChanges()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}