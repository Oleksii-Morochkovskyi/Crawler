using CrawlerLogic;

namespace ConsoleManager
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var console = new ConsoleProcessor();

            var input = console.GetAddress();

            var crawler = new CrawlerConfiguration();

            await crawler.ConfigureCrawlerAsync(input);
        }
    }
}