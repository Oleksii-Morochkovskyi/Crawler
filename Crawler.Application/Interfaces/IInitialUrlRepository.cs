using Crawler.Domain.Entities;

namespace Crawler.Application.Interfaces
{
    public interface IInitialUrlRepository
    {
        Task<InitialUrl> AddInitialUrlAsync(string baseUrl);
        Task<IEnumerable<InitialUrl>> GetInitialUrlsAsync();
    }
}
