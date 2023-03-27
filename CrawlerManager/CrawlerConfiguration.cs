using Microsoft.Extensions.DependencyInjection;

namespace CrawlerManager
{
    public class CrawlerConfiguration
    {
        public async Task ConfigureCrawler(string address)
        {
            var client = ConfigureIHttpClientFactory();

            var console = new CrawlerProcessor(client);

            await console.StartCrawler(address);
        }

        private HttpClient ConfigureIHttpClientFactory()
        {
            var services = new ServiceCollection();
            services.AddHttpClient();

            var serviceProvider = services.BuildServiceProvider();
            var clientFactory = serviceProvider.GetService<IHttpClientFactory>();

            return clientFactory.CreateClient();
        }
    }
}