
namespace ConsoleManager
{
    internal class Program
    {
        static async Task Main (string[] args)
        {
            var console = new ConsoleProcessor();
            
            await console.StartCrawler();
        }
    }
}