using Crawler.Logic;
using System.Net;
using ConsoleOutput.Wrappers;
using Crawler.Logic.Crawlers;
using Crawler.Logic.Helpers;
using Crawler.Logic.Parsers;
using Crawler.Logic.Services;
using Crawler.Logic.Validators;
using Microsoft.Graph.Communications.CallRecords.CallRecordsGetDirectRoutingCallsWithFromDateTimeWithToDateTime;

namespace ConsoleOutput
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var logger = new ConsoleWrapper();
            var validator = new UrlValidator();
            using var client = new HttpClient();
            var httpClientWrapper = new HttpClientWrapper(client);
            var helper = new UrlHelper();
            var tracker = new ResponseTimeTracker(httpClientWrapper, logger);
            var htmlParser = new HtmlParser(httpClientWrapper, helper);
            var htmlCrawler = new HtmlCrawler(logger, htmlParser, validator);

            var console = new ConsoleProcessor(logger, validator);

            var inputUrl = console.GetAddress();

            var crawler = new CrawlerProcessor(logger, helper, validator, tracker, htmlCrawler);

            var results = await crawler.StartCrawlerAsync(inputUrl);

            console.PrintResult(results);
        }
    }
}