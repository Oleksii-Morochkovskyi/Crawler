using Crawler.Logic.Crawlers;
using Crawler.Logic.Enums;
using Crawler.Logic.Interfaces;
using Crawler.Logic.Models;
using Crawler.Logic.Validators;
using System;
using System.ComponentModel.DataAnnotations;

namespace Crawler.ConsoleOutput
{
    public class ConsoleProcessor
    {
        private readonly IOHandler _iOHandler;
        private readonly UrlValidator _validator;
        private readonly Logic.Crawlers.Crawler _crawler;

        public ConsoleProcessor(IOHandler iOHandler, UrlValidator validator, Logic.Crawlers.Crawler crawler)
        {
            _iOHandler = iOHandler;
            _validator = validator;
            _crawler = crawler;
        }

        public async Task PrintResult()
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
                _iOHandler.Write("Enter URL: ");

                var input = _iOHandler.Read();

                if (_validator.IsValidUrl(input))
                {
                    return input.TrimEnd('/');
                }

                _iOHandler.Write("You have entered wrong url. Please try again...\n");
            }
        }

        private void PrintDifference(IEnumerable<UrlResponse> results)
        {
            var htmlExceptXml = results.Where(x => x.Location == Location.Html)
                .Select(x => x.Url);
            var xmlExceptHtml = results.Where(x => x.Location == Location.Xml)
                .Select(x => x.Url);

            if (!htmlExceptXml.Any() || !xmlExceptHtml.Any())
            {
                _iOHandler.Write("\nOne of the ways to search for links did not bring any result or two ways of crawling brought same results.\n");
                return;
            }

            _iOHandler.Write("\nUrls FOUND BY CRAWLING THE WEBSITE but not in sitemap.xml: \n");
            PrintList(htmlExceptXml);

            _iOHandler.Write("\nUrls FOUND IN SITEMAP.XML but not founded after crawling a website: \n");
            PrintList(xmlExceptHtml);
        }

        private void PrintList(IEnumerable<string> urls)
        {
            foreach (var url in urls)
            {
                _iOHandler.Write(url);
            }
        }

        private void PrintTimeResponse(IEnumerable<UrlResponse> urls)
        {
            _iOHandler.Write("\n\nList with url and response time for each page: \n");
            _iOHandler.Write("URL".PadRight(70) + "Timing (ms)\n");

            foreach (var url in urls)
            {
                _iOHandler.Write(url.Url.PadRight(70) + url.ResponseTimeMs + "ms");
            }
        }

        private void PrintNumberOfLinks(IEnumerable<UrlResponse> urls)
        {
            var countOfUrlsFromHtml = urls.Count(x => x.Location == Location.Html);
            var countOfUrlsFromXml = urls.Count(x => x.Location == Location.Xml);

            _iOHandler.Write($"\nUrls(html documents) found after crawling a website: {urls.Count() - countOfUrlsFromXml}");

            _iOHandler.Write($"\nUrls found in sitemap: {urls.Count() - countOfUrlsFromHtml}");
        }
    }
}
