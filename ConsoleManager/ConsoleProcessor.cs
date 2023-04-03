using Crawler.Logic.Enums;
using Crawler.Logic.Interfaces;
using Crawler.Logic.Models;
using Crawler.Logic.Validators;
using System.ComponentModel.DataAnnotations;

namespace ConsoleOutput
{
    public class ConsoleProcessor
    {
        private readonly IOutputWriter _writer;
        private readonly UrlValidator _validator;

        public ConsoleProcessor(IOutputWriter writer, UrlValidator validator)
        {
            _writer = writer;
            _validator = validator;
        }

        public string GetAddress()
        {
            while (true)
            {
                _writer.Write("Enter URL: ");

                var input = _writer.Read();

                if (_validator.IsValidUrl(input))
                {
                    return input.TrimEnd('/');
                }

                _writer.Write("You have entered wrong url. Please try again...\n");
            }
        }

        public void PrintResult(IEnumerable<UrlResponse> results)
        {
            PrintDifference(results);

            PrintTimeResponse(results);

            PrintNumberOfLinks(results);
        }

        private void PrintDifference(IEnumerable<UrlResponse> results)
        {
            var htmlExceptXml = results.Where(x => x.Location == Location.Html)
                .Select(x => x.Url);
            var xmlExceptHtml = results.Where(x => x.Location == Location.Xml)
                .Select(x => x.Url);

            if (!htmlExceptXml.Any() || !xmlExceptHtml.Any())
            {
                _writer.Write("\nOne of the ways to search for links did not bring any result or two ways of crawling brought same results.\n");
                return;
            }

            _writer.Write("\nUrls FOUND BY CRAWLING THE WEBSITE but not in sitemap.xml: \n");
            PrintList(htmlExceptXml);

            _writer.Write("\nUrls FOUND IN SITEMAP.XML but not founded after crawling a website: \n");
            PrintList(xmlExceptHtml);
        }

        private void PrintList(IEnumerable<string> urls)
        {
            foreach (var url in urls)
            {
                _writer.Write(url);
            }
        }

        private void PrintTimeResponse(IEnumerable<UrlResponse> urls)
        {
            _writer.Write("\n\nList with url and response time for each page: \n");
            _writer.Write("URL".PadRight(70) + "Timing (ms)\n");

            foreach (var url in urls)
            {
                _writer.Write(url.Url.PadRight(70) + url.ResponseTimeMs + "ms");
            }
        }

        private void PrintNumberOfLinks(IEnumerable<UrlResponse> urls)
        {
            var countOfUrlsFromHtml = urls.Count(x => x.Location == Location.Html);
            var countOfUrlsFromXml = urls.Count(x => x.Location == Location.Xml);

            _writer.Write($"\nUrls(html documents) found after crawling a website: {urls.Count() - countOfUrlsFromXml}");

            _writer.Write($"\nUrls found in sitemap: {urls.Count() - countOfUrlsFromHtml}");
        }

    }
}
