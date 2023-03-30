using Crawler.Logic;

namespace ConsoleManager
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var logger = new Logger();
            var console = new ConsoleProcessor(logger);
            var validator = new UrlValidator();

            var input = console.GetAddress();

            if (!validator.IsCorrectInput(input)) return;

            var crawler = new CrawlerConfiguration();

            var  resultSet = await crawler.ConfigureCrawlerAsync(input);

            console.PrintResult(resultSet);
        }
    }
}