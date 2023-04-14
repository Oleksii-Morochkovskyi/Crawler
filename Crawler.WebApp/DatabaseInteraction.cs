using Crawler.Persistence.Interfaces;

namespace Crawler.WebApp
{
    public class DatabaseInteraction
    {
        private readonly IFoundUrlRepository _foundUrlRepository;
        private readonly IInitialUrlRepository _initialUrlRepository;
        private readonly Logic.Crawlers.Crawler _crawler;

        public DatabaseInteraction(IFoundUrlRepository foundUrlRepository, IInitialUrlRepository initialUrlRepository, Logic.Crawlers.Crawler crawler)
        {
            _foundUrlRepository = foundUrlRepository;
            _initialUrlRepository = initialUrlRepository;
            _crawler = crawler;
        }

        public async Task<int> AddUrlsAsync(string baseUrl)
        {
            baseUrl = baseUrl.TrimEnd('/');

            var result = await _crawler.StartCrawlerAsync(baseUrl);

            var initialUrl = await _initialUrlRepository.AddInitialUrlAsync(baseUrl);

            return await _foundUrlRepository.AddFoundUrlsAsync(initialUrl, result);
        }
    }
}
