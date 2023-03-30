
using Crawler.Logic;
using Crawler.Logic.Enums;
using Crawler.Logic.Interfaces;
using Crawler.Logic.Models;

namespace ConsoleManager
{
    public class ConsoleProcessor
    {
        private readonly ILogger _logger;

        public ConsoleProcessor()
        {
            _logger = new Logger();
        }

        public string GetAddress()
        {
            _logger.Write("Enter URL: ");

            var input = _logger.Read();

            return input.TrimEnd('/');
        }

        public void PrintResult(IList<UrlResponse> resultSet)
        {
            PrintDifference(resultSet);

            PrintTimeResponse(resultSet);

            PrintNumberOfLinks(resultSet);
        }

        private void PrintDifference(IList<UrlResponse> resultSet)
        {
            var htmlExceptXml = resultSet.Where(x => x.location == Location.Html)
                                                            .Select(x=>x.Url);
            var xmlExceptHtml = resultSet.Where(x => x.location == Location.Xml)
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
            var urlsFromHtml = urls.Where(x => x.location == Location.Html);
            var urlsFromXml = urls.Where(x => x.location == Location.Xml);

            _logger.Write($"\nUrls(html documents) found after crawling a website: {urls.Count - urlsFromXml.Count()}");

            _logger.Write($"\nUrls found in sitemap: {urls.Count - urlsFromHtml.Count()}");
        }

    }
}
