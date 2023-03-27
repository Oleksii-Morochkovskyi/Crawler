
using Microsoft.Extensions.DependencyInjection;

namespace CrawlerManager
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var client = ConfigureIHttpClientFactory();

            var console = new CrawlerProcessor(client);

            await console.StartCrawler();
        }

        private static HttpClient ConfigureIHttpClientFactory()
        {
            var services = new ServiceCollection();
            services.AddHttpClient();

            var serviceProvider = services.BuildServiceProvider();
            var clientFactory = serviceProvider.GetService<IHttpClientFactory>();

            return clientFactory.CreateClient();
        }
    }
}