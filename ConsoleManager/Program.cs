using Crawler.Logic;
using System.Net;
using Crawler.Logic.Crawlers;
using Crawler.Logic.Helpers;
using Crawler.Logic.Parsers;
using Crawler.Logic.Services;
using Crawler.Logic.Validators;

namespace ConsoleOutput
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var logger = new ConsoleIO();
            var validator = new UrlValidator();
            using var client = new HttpClient();
            var helper = new UrlHelper();
            var tracker = new ResponseTimeTracker(client, logger);
            var htmlParser = new HtmlParser(client, helper, validator);
            var htmlCrawler = new HtmlCrawler(logger, htmlParser);

            var console = new ConsoleProcessor(logger);

            var inputUrl = console.GetAddress();

            if (!validator.IsValidUrl(inputUrl)) return;

            var crawler = new CrawlerProcessor(logger, helper, validator, tracker, htmlCrawler);

            var  results = await crawler.StartCrawlerAsync(inputUrl);

            console.PrintResult(results);
        }
    }
}