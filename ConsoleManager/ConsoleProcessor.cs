
using Crawler.Logic;
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

        public void PrintResult(ResultSet resultSet)
        {
            PrintDifference(resultSet.htmlExceptXml, resultSet.xmlExceptHtml);

            PrintTimeResponse(resultSet.urlResponses);

            PrintNumberOfLinks(resultSet.urlsFromHtml, resultSet.urlsFromXml);
        }

        private void PrintDifference(ICollection<string> htmlExceptXml, ICollection<string> xmlExceptHtml)
        {
            if (htmlExceptXml.Count == 0 || xmlExceptHtml.Count == 0)
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
            _logger.Write("URL".PadRight(50) + "Timing (ms)\n");

            foreach (var url in urls)
            {
                _logger.Write(url.Url.PadRight(50) + url.ResponseTime + "ms"); //print the result
            }
        }

        private void PrintNumberOfLinks(ICollection<string> urlsFromHtmlCrawling, ICollection<string> urlsFromXmlCrawling)
        {
            _logger.Write($"\nUrls(html documents) found after crawling a website: {urlsFromHtmlCrawling.Count}");

            _logger.Write($"\nUrls found in sitemap: {urlsFromXmlCrawling.Count}");
        }

    }
}
