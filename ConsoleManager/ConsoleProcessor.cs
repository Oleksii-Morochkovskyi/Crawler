using Crawler.Logic.Enums;
using Crawler.Logic.Interfaces;
using Crawler.Logic.Models;
using Crawler.Logic.Validators;

namespace Crawler.ConsoleOutput
{
    public class ConsoleProcessor
    {
        private readonly IConsoleHandler _consoleHandler;
        private readonly UrlValidator _validator;
        private readonly Logic.Crawlers.Crawler _crawler;

        public ConsoleProcessor(IConsoleHandler consoleHandler, UrlValidator validator, Logic.Crawlers.Crawler crawler)
        {
            _consoleHandler = consoleHandler;
            _validator = validator;
            _crawler = crawler;
        }

        public async Task ExecuteAsync()
        {
            var inputUrl = GetAddress();
            
            var results = await _crawler.StartCrawlerAsync(inputUrl);
            
            PrintDifference(results);

            PrintTimeResponse(results);

            PrintNumberOfLinks(results);
        }

        public string GetAddress()
        {
            while (true)
            {
                _consoleHandler.Write("Enter URL: ");

                var input = _consoleHandler.Read();

                if (_validator.IsValidUrl(input))
                {
                    return input.TrimEnd('/');
                }

                _consoleHandler.Write("You have entered wrong url. Please try again...\n");
            }
        }

        private void PrintDifference(IEnumerable<UrlResponse> results)
        {
            var htmlExceptXml = results.Where(x => x.Location == Location.Html)
                .Select(x => x.Url);
            var xmlExceptHtml = results.Where(x => x.Location == Location.Xml)
                .Select(x => x.Url);

            _consoleHandler.Write("\nUrls FOUND BY CRAWLING THE WEBSITE but not in sitemap.xml: \n");
            PrintList(htmlExceptXml);

            _consoleHandler.Write("\nUrls FOUND IN SITEMAP.XML but not founded after crawling a website: \n");
            PrintList(xmlExceptHtml);
        }

        private void PrintList(IEnumerable<string> urls)
        {
            foreach (var url in urls)
            {
                _consoleHandler.Write(url);
            }
        }

        private void PrintTimeResponse(IEnumerable<UrlResponse> urls)
        {
            _consoleHandler.Write("\n\nList with url and response time for each page: \n");
            _consoleHandler.Write("URL".PadRight(70) + "Timing (ms)\n");

            foreach (var url in urls)
            {
                _consoleHandler.Write(url.Url.PadRight(70) + url.ResponseTimeMs + "ms");
            }
        }

        private void PrintNumberOfLinks(IEnumerable<UrlResponse> urls)
        {
            var countOfUrlsFromHtml = urls.Count(x => x.Location == Location.Html);
            var countOfUrlsFromXml = urls.Count(x => x.Location == Location.Xml);

            _consoleHandler.Write($"\nUrls(html documents) found after crawling a website: {urls.Count() - countOfUrlsFromXml}");

            _consoleHandler.Write($"\nUrls found in sitemap: {urls.Count() - countOfUrlsFromHtml}");
        }
    }
}
