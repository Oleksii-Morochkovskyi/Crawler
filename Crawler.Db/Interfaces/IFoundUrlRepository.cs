using Crawler.Logic.Models;
using Crawler.Persistence.Entities;

namespace Crawler.Persistence.Interfaces
{
    public interface IFoundUrlRepository
    {
        Task AddFoundUrlsAsync(InitialUrl initialUrl, IEnumerable<UrlResponse> urls);
    }
}
