using Crawler.Logic.Interfaces;
using Crawler.Logic.Parsers;
using System;

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
            ICollection<string> checkedUrls = new HashSet<string>();
            ICollection<string> urlsToCheck = new HashSet<string>{ baseUrl };

            while (urlsToCheck.Count > 0)
            {
                try
                {
                    var url = urlsToCheck.First();

                    urlsToCheck = await _parser.ParseAsync(baseUrl, url, checkedUrls, urlsToCheck);

                    checkedUrls.Add(url);

                    urlsToCheck.Remove(url);
                }
                catch (Exception e)
                {
                    checkedUrls.Add(urlsToCheck.First());
                    urlsToCheck.Remove(urlsToCheck.First());

                    _logger.Write(e.Message);
                }
            }

            return checkedUrls;
        }
    }
}
