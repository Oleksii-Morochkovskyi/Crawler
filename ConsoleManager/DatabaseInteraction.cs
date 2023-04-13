using Crawler.Logic.Models;
using Crawler.Persistence.Interfaces;

namespace Crawler.ConsoleOutput
{
    public class DatabaseInteraction
    {
        private readonly IFoundUrlRepository _foundUrlRepository;
        private readonly IInitialUrlRepository _initialUrlRepository;

        public DatabaseInteraction(IFoundUrlRepository foundUrlRepository, IInitialUrlRepository initialUrlRepository)
        {
            _foundUrlRepository = foundUrlRepository;
            _initialUrlRepository = initialUrlRepository;
        }

        public async Task<int> AddUrlsAsync(IEnumerable<UrlResponse> urls, string baseUrl)
        {
            var initialUrl = await _initialUrlRepository.AddInitialUrlAsync(baseUrl);

            return await _foundUrlRepository.AddFoundUrlsAsync(initialUrl, urls);
        }
    }
}
