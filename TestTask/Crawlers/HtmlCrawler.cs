using CrawlerLogic.Parsers;

namespace CrawlerLogic.Crawlers
{
    public class HtmlCrawler
    {
        private HashSet<string> _urls;
        private readonly HttpClient _httpClient;
        private readonly HtmlParser _parser;

        public HtmlCrawler(string address, HttpClient client)
        {
            _httpClient = client;

            _parser = new HtmlParser(_httpClient);

            _urls = new HashSet<string>();
        }

        public async Task<HashSet<string>> CrawlUrlAsync(string address)
        {
            try
            {
                var urls = await _parser.ParseUrlAsync(address);

                foreach (var url in urls)
                {
                    if (_urls.Contains(url))
                    {
                        continue;
                    }

                    _urls.Add(url);

                    await CrawlUrlAsync(url);
                }
            }
            catch (Exception) { }

            return _urls;
        }
    }
}
