using Crawler.Domain.Entities;

namespace Crawler.Application.Interfaces
{
    public interface IFoundUrlRepository
    {
        Task<int> AddFoundUrlsAsync(InitialUrl initialUrl, IEnumerable<UrlResponse> urls);
        Task<IEnumerable<FoundUrl>> GetUrlsByInitialUrlIdAsync(int id);
    }
}
