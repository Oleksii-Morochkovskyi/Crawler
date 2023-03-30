using Crawler.Logic.Enums;
using Crawler.Logic.Interfaces;
using Crawler.Logic.Models;

namespace ConsoleOutput
{
    public class ConsoleProcessor
    {
        private readonly ILogger _logger;

        public ConsoleProcessor(ILogger logger)
        {
            _logger = logger;
        }

        public string GetAddress()
        {
            _logger.Write("Enter URL: ");

            var input = _logger.Read();

            return input.TrimEnd('/');
        }

        public void PrintResult(IList<UrlResponse> results)
        {
            PrintDifference(results);

            PrintTimeResponse(results);

            PrintNumberOfLinks(results);
        }

        private void PrintDifference(IList<UrlResponse> results)
        {
            var htmlExceptXml = results.Where(x => x.Location == Location.Html)
                .Select(x=>x.Url);
            var xmlExceptHtml = results.Where(x => x.Location == Location.Xml)
                .Select(x => x.Url);
            
            if (!htmlExceptXml.Any() || !xmlExceptHtml.Any())
            {
                _logger.Write("\nOne of the ways to search for links did not bring any result or two ways of crawling brought same results.\n");
                return;
            }

            _logger.Write("\nUrls FOUND BY CRAWLING THE WEBSITE but not in sitemap.xml: \n");
            PrintList(htmlExceptXml);

            _logger.Write("\nUrls FOUND IN SITEMAP.XML but not founded after crawling a website: \n");
            PrintList(xmlExceptHtml);
        }

        private void PrintList(IEnumerable<string> urls)
        {
            foreach (var url in urls)
            {
                _logger.Write(url);
            }
        }

        private void PrintTimeResponse(IList<UrlResponse> urls)
        {
            _logger.Write("\n\nList with url and response time for each page: \n");
            _logger.Write("URL".PadRight(70) + "Timing (ms)\n");

            foreach (var url in urls)
            {
                _logger.Write(url.Url.PadRight(70) + url.ResponseTime + "ms"); 
            }
        }

        private void PrintNumberOfLinks(IList<UrlResponse> urls)
        {
            var countOfUrlsFromHtml = urls.Count(x => x.Location == Location.Html);
            var countOfUrlsFromXml = urls.Count(x => x.Location == Location.Xml);

            _logger.Write($"\nUrls(html documents) found after crawling a website: {urls.Count - countOfUrlsFromXml}");

            _logger.Write($"\nUrls found in sitemap: {urls.Count - countOfUrlsFromHtml}");
        }

    }
}
