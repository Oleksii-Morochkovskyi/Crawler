
using Crawler.Logic.Models;

namespace Crawler.UrlRepository.Repositories
{
    public interface IInitialUrlRepository
    {
        Task AddInitialUrlAsync(string baseUrl);
    }
}
