using Crawler.Logic.Validators;
using Crawler.Application.Wrappers;
using Crawler.Logic.Parsers;

namespace Crawler.Logic.Crawlers
{
    public class HtmlCrawler
    {
        private readonly HtmlParser _parser;
        private readonly ConsoleWrapper _consoleWrapper;
        private readonly UrlValidator _urlValidator;

        public HtmlCrawler(
            ConsoleWrapper consoleWrapper,
            HtmlParser parser,
            UrlValidator validator)
        {
            _consoleWrapper = consoleWrapper;
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

                    _consoleWrapper.Write(e.Message);
                }
            }

            return checkedUrls;
        }

        private ICollection<string> FilterLinks(string baseUrl, ICollection<string> checkedUrls,
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