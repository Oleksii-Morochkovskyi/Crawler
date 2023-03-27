using CrawlerLogic;
using CrawlerLogic.Crawlers;

namespace CrawlerManager
{
    public class CrawlerProcessor
    {
        private readonly HttpClient _httpClient;
        private readonly CrawlerOutput _crawlerOutput;

        public CrawlerProcessor(HttpClient client)
        {
            _crawlerOutput = new CrawlerOutput();
            _httpClient = client;
        }

        public async Task StartCrawler(string address)
        {
            var urlListFromHtmlCrawler = await StartHtmlCrawler(address);

            var urlListFromXmlCrawler = await StartXmlCrawler(address);

            var allUrls = urlListFromHtmlCrawler.Union(urlListFromXmlCrawler);

            CompareCrawlResults(urlListFromHtmlCrawler, urlListFromXmlCrawler);

            await GetResponseTime(allUrls);

            _crawlerOutput.PrintNumberOfLinks(urlListFromHtmlCrawler, urlListFromXmlCrawler);
        }

        private async Task<ICollection<string>> StartHtmlCrawler(string address)
        {
            var htmlCrawler = new HtmlCrawler(address, _httpClient);
            var manager = new UrlManager(address, _httpClient);

            try
            {
                if (!manager.CheckUrl(address))
                {
                    throw new Exception();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"\nOoooops, seems like you entered wrong url. {e.Message}. Please, try again.");
            }

            return await htmlCrawler.ParseUrl(address);
        }

        private async Task<ICollection<string>> StartXmlCrawler(string address)
        {
            address += "/sitemap.xml";

            var xmlCrawler = new XmlCrawler(_httpClient, address);

            return await xmlCrawler.ParseUrl(address);
        }

        private void CompareCrawlResults(ICollection<string> urlsFromHtmlCrawling, ICollection<string> urlsFromXmlCrawling)
        {
            if (urlsFromXmlCrawling.Count == 0 || urlsFromHtmlCrawling.Count == 0)
            {
                Console.WriteLine("\nOne of the ways to search for links did not bring any result.\n");
            }
            else
            {
                ShowDifferentUrls(urlsFromHtmlCrawling, urlsFromXmlCrawling);
            }
        }

        private void ShowDifferentUrls(ICollection<string> urlsFromHtmlCrawling, ICollection<string> urlsFromXmlCrawling)
        {
            if (urlsFromHtmlCrawling.Equals(urlsFromXmlCrawling))
            {
                Console.WriteLine("\nThere is no difference between the links obtained by the two methods");

                _crawlerOutput.PrintList(urlsFromHtmlCrawling);

                return;
            }

            var differenceList = urlsFromHtmlCrawling.Except(urlsFromXmlCrawling);

            Console.WriteLine("\nUrls FOUND BY CRAWLING THE WEBSITE but not in sitemap.xml: \n");
            _crawlerOutput.PrintList(differenceList);

            differenceList = urlsFromXmlCrawling.Except(urlsFromHtmlCrawling);

            Console.WriteLine("\nUrls FOUND IN SITEMAP.XML but not founded after crawling a website: \n");
            _crawlerOutput.PrintList(differenceList);

        }

        private async Task GetResponseTime(IEnumerable<string> urlList)
        {
            var tracker = new ResponseTimeTracker(_httpClient);

            var responseTime = await tracker.GetResponseTime(urlList);

            _crawlerOutput.PrintTimeResponse(responseTime);
        }
    }
}
