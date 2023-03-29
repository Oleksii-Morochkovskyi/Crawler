using Crawler.Logic.Models;

namespace Crawler.Logic
{
    public class CrawlerConfiguration
    {
        public async Task<ResultSet> ConfigureCrawlerAsync(string address)
        {
            using var client = new HttpClient();

            var console = new CrawlerProcessor(client);
            
            return await console.StartCrawlerAsync(address);
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