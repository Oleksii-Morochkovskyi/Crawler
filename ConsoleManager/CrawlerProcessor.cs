using CrawlerLogic.Crawlers;
using CrawlerLogic;

namespace CrawlerManager
{
    internal class CrawlerProcessor
    {
        private readonly HttpClient _httpClient;

        public CrawlerProcessor(HttpClient client)
        {
            _httpClient = client;
        }

        public async Task StartCrawler()
        {
            var address = GetAddress();

            var urlListFromHtmlCrawler = await StartHtmlCrawler(address);

            var urlListFromXmlCrawler = await StartXmlCrawler(address);

            var allUrls = urlListFromHtmlCrawler.Union(urlListFromXmlCrawler);

            CompareCrawlResults(urlListFromHtmlCrawler, urlListFromXmlCrawler);

            await GetResponseTime(allUrls);

            PrintNumberOfLinks(urlListFromHtmlCrawler, urlListFromXmlCrawler);
        }

        private string GetAddress()
        {
            Console.WriteLine("Enter URL: ");

            var input = Console.ReadLine();

            return input.TrimEnd('/');
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

                PrintList(urlsFromHtmlCrawling);

                return;
            }

            var differenceList = urlsFromHtmlCrawling.Except(urlsFromXmlCrawling);

            Console.WriteLine("\nUrls FOUND BY CRAWLING THE WEBSITE but not in sitemap.xml: \n");
            PrintList(differenceList);

            differenceList = urlsFromXmlCrawling.Except(urlsFromHtmlCrawling);

            Console.WriteLine("\nUrls FOUND IN SITEMAP.XML but not founded after crawling a website: \n");
            PrintList(differenceList);

        }

        private void PrintList(IEnumerable<string> urlList)
        {
            foreach (var url in urlList)
            {
                Console.WriteLine(url);
            }
        }

        private async Task GetResponseTime(IEnumerable<string> urlList)
        {
            var tracker = new ResponseTimeTracker(_httpClient);

            var responseTime = await tracker.GetResponseTime(urlList);

            PrintTimeResponse(responseTime);
        }

        private void PrintTimeResponse(Dictionary<string, int> sortedDict)
        {
            Console.WriteLine("\n\nList with url and response time for each page: \n");
            Console.WriteLine("\nURL".PadRight(50) + "Timing (ms)");

            foreach (var pair in sortedDict)
            {
                Console.WriteLine(pair.Key.PadRight(50) + pair.Value + "ms"); //print the result
            }
        }

        private void PrintNumberOfLinks(ICollection<string> urlsFromHtmlCrawling, ICollection<string> urlsFromXmlCrawling)
        {
            Console.WriteLine($"\nUrls(html documents) found after crawling a website: {urlsFromHtmlCrawling.Count}");

            Console.WriteLine($"\nUrls found in sitemap: {urlsFromXmlCrawling.Count}");
        }
    }
}
