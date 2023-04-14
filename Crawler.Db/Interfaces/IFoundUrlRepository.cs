using Crawler.Logic.Models;
using Crawler.Persistence.Entities;

namespace Crawler.Persistence.Interfaces
{
    public interface IFoundUrlRepository
    {
        Task<int> AddFoundUrlsAsync(InitialUrl initialUrl, IEnumerable<UrlResponse> urls);
        Task<IEnumerable<FoundUrl>> GetUrlsByInitialUrlIdAsync(int id);
    }
}
