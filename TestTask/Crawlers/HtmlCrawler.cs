using Crawler.Logic.Parsers;
using IOManager;

namespace Crawler.Logic.Crawlers
{
    public class HtmlCrawler
    {
        private readonly HttpClient _httpClient;
        private readonly HtmlParser _parser;
        private readonly ILogger _logger;

        public HtmlCrawler(string address, HttpClient client)
        {
            _httpClient = client;
            _logger = new Logger();
            _parser = new HtmlParser(_httpClient, address);
        }

        public async Task<ICollection<string>> CrawlAsync(string address)
        {
            ICollection<string> urls = new HashSet<string>();
            
            return await CrawlAsync(address, urls);
        }

        private async Task<ICollection<string>> CrawlAsync(string address, ICollection<string> checkedUrls)
        {
            try
            {
                var urls = await _parser.ParseAsync(address);

                foreach (var url in urls)
                {
                    if (checkedUrls.Contains(url))
                    {
                        continue;
                    }

                    checkedUrls.Add(url);

                    checkedUrls = await CrawlAsync(url, checkedUrls);
                }
            }
            catch (Exception e)
            {
                _logger.Write(e.Message);
            }

            return checkedUrls;
        }
    }
}
