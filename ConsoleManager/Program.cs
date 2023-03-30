using Crawler.Logic;

namespace ConsoleManager
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var logger = new Logger();
            var validator = new UrlValidator();
            var console = new ConsoleProcessor(logger);

            var inputUrl = console.GetAddress();

            if (!validator.IsInputUrlCorrect(inputUrl)) return;

            var crawler = new CrawlerConfiguration();

            var  resultSet = await crawler.ConfigureCrawlerAsync(inputUrl);

            console.PrintResult(resultSet);
        }
    }
}