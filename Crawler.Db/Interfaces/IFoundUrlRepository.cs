using Crawler.Logic.Models;

namespace Crawler.UrlRepository.Interfaces
{
    public interface IFoundUrlRepository
    {
        Task AddFoundUrlsAsync(IEnumerable<UrlResponse> urls);
    }
}
