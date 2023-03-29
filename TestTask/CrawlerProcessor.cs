using Crawler.Logic.Crawlers;
using Crawler.Logic.Models;

namespace Crawler.Logic
{
    public class CrawlerProcessor
    {
        private readonly HttpClient _httpClient;

        public CrawlerProcessor(HttpClient client)
        {
            _httpClient = client;
        }

        public async Task<ResultSet> StartCrawlerAsync(string address)
        {
            var urlsFromHtmlCrawler = await StartHtmlCrawlerAsync(address);

            var urlsFromXmlCrawler = await StartXmlCrawlerAsync(address);

            var allUrls = urlsFromHtmlCrawler.Union(urlsFromXmlCrawler);

            var urlsInHtmlExceptXml = urlsFromHtmlCrawler.Except(urlsFromXmlCrawler)
                                                                        .ToHashSet();
            var urlsInXmlExceptHtml = urlsFromXmlCrawler.Except(urlsFromHtmlCrawler)
                                                                        .ToHashSet();

            var responseTime = await GetResponseTimeAsync(allUrls);

            var resultSet = new ResultSet
            {
                urlsFromHtml = urlsFromHtmlCrawler,
                urlsFromXml = urlsFromXmlCrawler,
                htmlExceptXml = urlsInHtmlExceptXml,
                xmlExceptHtml = urlsInXmlExceptHtml,
                urlResponses = responseTime

            }; 

            return resultSet;
        }

        private async Task<ICollection<string>> StartHtmlCrawlerAsync(string address)
        {
            var htmlCrawler = new HtmlCrawler(address, _httpClient);

            return await htmlCrawler.CrawlAsync(address);
        }

        private async Task<ICollection<string>> StartXmlCrawlerAsync(string address)
        {
            var xmlCrawler = new XmlCrawler(address);

            return await xmlCrawler.CrawlAsync(address);
        }

        private async Task<IList<UrlResponse>> GetResponseTimeAsync(IEnumerable<string> urls)
        {
            var tracker = new ResponseTimeTracker(_httpClient);

            return await tracker.GetResponseTimeAsync(urls);
        }
    }
}
