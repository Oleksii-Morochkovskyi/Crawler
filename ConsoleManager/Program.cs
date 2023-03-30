using Crawler.Logic;
using System.Net;
using Crawler.Logic.Crawlers;
using Crawler.Logic.Parsers;
using Crawler.Logic.TimeTracker;
using Crawler.Logic.UrlTools;

namespace ConsoleManager
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var logger = new Logger();
            var validator = new UrlValidator();
            using var client = new HttpClient();
            var helper = new UrlHelper();
            var tracker = new ResponseTimeTracker(client, logger);
            var htmlParser = new HtmlParser(client, helper, validator);
            var htmlCrawler = new HtmlCrawler(logger, htmlParser);

            var console = new ConsoleProcessor(logger);

            var inputUrl = console.GetAddress();

            if (!validator.IsInputUrlCorrect(inputUrl)) return;

            var crawler = new Crawler.Logic.Crawler(logger, helper, validator, tracker, htmlCrawler);

            var  resultSet = await crawler.StartCrawlerAsync(inputUrl);

            console.PrintResult(resultSet);
        }
    }
}