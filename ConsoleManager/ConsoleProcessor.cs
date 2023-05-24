using Crawler.Application.Crawlers.Interfaces;
using Crawler.ConsoleOutput.Wrappers;
using Crawler.Domain.Entities;
using Crawler.Domain.Enums;
using Crawler.Logic.Validators;

namespace Crawler.ConsoleOutput
{
    public class ConsoleProcessor
    {
        private readonly ConsoleWrapper _consoleWrapper;
        private readonly UrlValidator _validator;
        private readonly ICrawler _crawler;
        private readonly DatabaseInteraction _dbInteraction;

        public ConsoleProcessor(
            ConsoleWrapper consoleWrapper,
            UrlValidator validator,
            ICrawler crawler,
            DatabaseInteraction dbInteraction)
        {
            _consoleWrapper = consoleWrapper;
            _validator = validator;
            _crawler = crawler;
            _dbInteraction = dbInteraction;
        }

        public async Task ExecuteAsync()
        {
            try
            {
                var inputUrl = GetAddress();

                var results = await _crawler.StartCrawlerAsync(inputUrl);

                PrintDifference(results);

                PrintTimeResponse(results);

                PrintNumberOfLinks(results);

                await _dbInteraction.AddUrlsAsync(results, inputUrl);
            }
            catch (Exception e)
            {
                _consoleWrapper.Write(e.Message);
            }
        }

        public string GetAddress()
        {
            while (true)
            {
                _consoleWrapper.Write("Enter URL: ");

                var input = _consoleWrapper.Read();

                if (_validator.IsValidUrl(input))
                {
                    return input.TrimEnd('/');
                }

                _consoleWrapper.Write("You have entered wrong url. Please try again...\n");
            }
        }

        private void PrintDifference(IEnumerable<UrlResponse> results)
        {
            var htmlExceptXml = results.Where(x => x.Location == Location.Html)
                .Select(x => x.Url);
            var xmlExceptHtml = results.Where(x => x.Location == Location.Xml)
                .Select(x => x.Url);

            _consoleWrapper.Write("\nUrls FOUND BY CRAWLING THE WEBSITE but not in sitemap.xml: \n");
            PrintList(htmlExceptXml);

            _consoleWrapper.Write("\nUrls FOUND IN SITEMAP.XML but not founded after crawling a website: \n");
            PrintList(xmlExceptHtml);
        }

        private void PrintList(IEnumerable<string> urls)
        {
            foreach (var url in urls)
            {
                _consoleWrapper.Write(url);
            }
        }

        private void PrintTimeResponse(IEnumerable<UrlResponse> urls)
        {
            _consoleWrapper.Write("\n\nList with url and response time for each page: \n");
            _consoleWrapper.Write("URL".PadRight(70) + "Timing (ms)\n");

            foreach (var url in urls)
            {
                _consoleWrapper.Write(url.Url.PadRight(70) + url.ResponseTimeMs + "ms");
            }
        }

        private void PrintNumberOfLinks(IEnumerable<UrlResponse> urls)
        {
            var countOfUrlsFromHtml = urls.Count(x => x.Location == Location.Html);
            var countOfUrlsFromXml = urls.Count(x => x.Location == Location.Xml);

            _consoleWrapper.Write($"\nUrls(html documents) found after crawling a website: {urls.Count() - countOfUrlsFromXml}");

            _consoleWrapper.Write($"\nUrls found in sitemap: {urls.Count() - countOfUrlsFromHtml}");
        }
    }
}
