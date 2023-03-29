using Crawler.Logic.Interfaces;
using Crawler.Logic.Parsers;

namespace Crawler.Logic.Crawlers
{
    public class HtmlCrawler
    {
        private readonly HtmlParser _parser;
        private readonly ILogger _logger;

        public HtmlCrawler(ILogger logger, HtmlParser parser)
        {
            _logger = logger;
            _parser = parser;
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
