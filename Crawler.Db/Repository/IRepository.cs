
using Crawler.Logic.Models;
using Microsoft.EntityFrameworkCore;

namespace Crawler.Db.Repository
{
    public interface IRepository
    {
        void AddFoundUrls(IEnumerable<UrlResponse> urls, string baseUrl);
        Task SaveChanges();
        DbContextOptions<CrawlerDbContext> ConfigureRepository();
    }
}
