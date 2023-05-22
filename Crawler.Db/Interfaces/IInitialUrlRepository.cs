using Crawler.Persistence.Entities;

namespace Crawler.Persistence.Interfaces
{
    public interface IInitialUrlRepository
    {
        Task<InitialUrl> AddInitialUrlAsync(string baseUrl);
        Task<IEnumerable<InitialUrl>> GetInitialUrlsAsync();
    }
}
