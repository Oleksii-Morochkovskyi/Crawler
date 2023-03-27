using Microsoft.Extensions.DependencyInjection;

namespace CrawlerLogic
{
    public class CrawlerConfiguration
    {
        public async Task ConfigureCrawlerAsync(string address)
        {
            using var client = new HttpClient();

            var console = new CrawlerProcessor(client);

            await console.StartCrawlerAsync(address);
        }

        /*private HttpClient ConfigureIHttpClientFactory()
        {
            var services = new ServiceCollection();
            services.AddHttpClient();

            var serviceProvider = services.BuildServiceProvider();
            var clientFactory = serviceProvider.GetService<IHttpClientFactory>();

            return clientFactory.CreateClient();
        }*/
    }
}