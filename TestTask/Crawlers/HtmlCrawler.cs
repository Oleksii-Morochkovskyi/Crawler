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

        public async Task<ICollection<string>> CrawlAsync(string baseUrl)
        {
            ICollection<string> urls = new HashSet<string>();
            ICollection<string> urlsToCheck = new HashSet<string>();

            try
            {
                urlsToCheck.Add(baseUrl);

                while (urlsToCheck.Count > 0)
                {
                    urlsToCheck = await _parser.ParseAsync(baseUrl, urlsToCheck.First(), urls, urlsToCheck);

                    urls.Add(urlsToCheck.First());

                    urlsToCheck.Remove(urlsToCheck.First());
                }
            }
            catch (Exception e)
            {
                _logger.Write(e.Message);
            }

            return urls;
        }
    }
}
