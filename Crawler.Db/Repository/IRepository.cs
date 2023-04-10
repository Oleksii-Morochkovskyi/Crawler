
using Crawler.Logic.Models;

namespace Crawler.UrlDatabase.Repository
{
    public interface IRepository
    {
        Task AddFoundUrlsAsync(IEnumerable<UrlResponse> urls, string baseUrl);
        Task AddInitialUrlAsync(string baseUrl);
    }
}
