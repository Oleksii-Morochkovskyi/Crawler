using Crawler.Logic.Crawlers;
using Crawler.Logic.Helpers;
using Crawler.Logic.Parsers;
using Crawler.Logic.Services;
using Crawler.Logic.Validators;

namespace Crawler.ConsoleOutput
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var consoleHandler = new ConsoleWrapper();
            var validator = new UrlValidator();
            using var client = new HttpClient();
            var httpClient = new HttpClientService(client);
            var helper = new UrlHelper();
            var tracker = new ResponseTimeService(httpClient, consoleHandler);
            var htmlParser = new HtmlParser(httpClient, helper);
            var htmlCrawler = new HtmlCrawler(consoleHandler, htmlParser, validator);
            var xmlCrawler = new XmlCrawler(consoleHandler, helper, validator);
            var crawler = new Crawler.Logic.Crawlers.Crawler(tracker, htmlCrawler, xmlCrawler);
            var console = new ConsoleProcessor(consoleHandler, validator, crawler);

            await console.PrintResult();
        }
    }
}