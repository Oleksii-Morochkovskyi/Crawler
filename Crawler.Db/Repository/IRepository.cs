
using Crawler.Logic.Models;

namespace Crawler.Db.Repository
{
    public interface IRepository
    {
        void AddFoundUrls(IEnumerable<UrlResponse> urls, string baseUrl);
        Task SaveChanges();
    }
}
