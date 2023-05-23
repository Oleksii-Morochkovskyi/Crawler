using Crawler.Application.Crawlers.Interfaces;
using Crawler.Application.Interfaces;
using Crawler.Application.Parsers.Interfaces;
using Crawler.Application.Validators;

namespace Crawler.Logic.Crawlers
{
    public class HtmlCrawler : IHtmlCrawler
    {
        private readonly IHtmlParser _parser;
        private readonly IConsoleHandler _consoleHandler;
        private readonly UrlValidator _urlValidator;

        public HtmlCrawler(
            IConsoleHandler consoleHandler,
            IHtmlParser parser,
            UrlValidator validator)
        {
            _consoleHandler = consoleHandler;
            _parser = parser;
            _urlValidator = validator;
        }

        public virtual async Task<ICollection<string>> CrawlAsync(string baseUrl)
        {
            ICollection<string> checkedUrls = new HashSet<string>();
            ICollection<string> urlsToCheck = new HashSet<string> { baseUrl };

            while (urlsToCheck.Count > 0)
            {
                try
                {
                    var url = urlsToCheck.First();

                    var parsedUrls = await _parser.ParseAsync(baseUrl, url);

                    checkedUrls.Add(url);

                    urlsToCheck.Remove(url);

                    urlsToCheck = FilterLinks(baseUrl, checkedUrls, urlsToCheck, parsedUrls);
                }
                catch (Exception e)
                {
                    checkedUrls.Add(urlsToCheck.First());

                    urlsToCheck.Remove(urlsToCheck.First());

                    _consoleHandler.Write(e.Message);
                }
            }

            return checkedUrls;
        }

        public ICollection<string> FilterLinks(string baseUrl, ICollection<string> checkedUrls,
            ICollection<string> urlsToCheck, ICollection<string> parsedUrls)
        {
            foreach (var url in parsedUrls)
            {
                var isChecked = urlsToCheck.Contains(url) || checkedUrls.Contains(url);

                if (!isChecked && _urlValidator.IsCorrectFormat(url, baseUrl) && _urlValidator.IsHtmlDoc(url, baseUrl))
                {
                    urlsToCheck.Add(url);
                }
            }

            return urlsToCheck;
        }
    }
}