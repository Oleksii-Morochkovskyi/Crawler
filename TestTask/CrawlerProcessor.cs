using CrawlerLogic.Crawlers;

namespace CrawlerLogic
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

        public async Task StartCrawlerAsync(string address)
        {
            var urlListFromHtmlCrawler = await StartHtmlCrawlerAsync(address);

            var urlListFromXmlCrawler = await StartXmlCrawlerAsync(address);

            var allUrls = urlListFromHtmlCrawler.Union(urlListFromXmlCrawler);

            CompareCrawlResults(urlListFromHtmlCrawler, urlListFromXmlCrawler);

            await GetResponseTimeAsync(allUrls);

            _crawlerOutput.PrintNumberOfLinks(urlListFromHtmlCrawler, urlListFromXmlCrawler);
        }

        private async Task<ICollection<string>> StartHtmlCrawlerAsync(string address)
        {
            var htmlCrawler = new HtmlCrawler(address, _httpClient);
            var validator = new UrlValidator(address, _httpClient);

            try
            {
                if (!validator.IsValidUrl(address))
                {
                    throw new Exception();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"\nOoooops, seems like you entered wrong url. {e.Message}. Please, try again.");
            }

            return await htmlCrawler.ParseUrlAsync(address);
        }

        private async Task<ICollection<string>> StartXmlCrawlerAsync(string address)
        {
            address += "/sitemap.xml";

            var xmlCrawler = new XmlCrawler(_httpClient, address);

            return await xmlCrawler.ParseUrlAsync(address);
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

        private async Task GetResponseTimeAsync(IEnumerable<string> urls)
        {
            var tracker = new ResponseTimeTracker(_httpClient);

            var responseTime = await tracker.GetResponseTimeAsync(urls);

            _crawlerOutput.PrintTimeResponse(responseTime);
        }
    }
}
