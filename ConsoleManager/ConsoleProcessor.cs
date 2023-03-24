using CrawlerLogic.Crawlers;
using CrawlerLogic;
using Timer = CrawlerLogic.Timer;

namespace ConsoleManager
{
    internal class ConsoleProcessor
    {
        public async Task StartCrawler()
        {
            var address = GetAddress();

            var urlListFromHtmlCrawler = await StartHtmlCrawler(address);

            var urlListFromXmlCrawler = await StartXmlCrawler(address);

            var allUrls = urlListFromHtmlCrawler.Union(urlListFromXmlCrawler);

            

            await GetResponseTime(allUrls);

            PrintNumberOfLinks(urlListFromHtmlCrawler, urlListFromXmlCrawler);
        }

        private string GetAddress()
        {
            Console.WriteLine("Enter URL: ");

            var input = Console.ReadLine();

            return input.TrimEnd('/');
        }

        private async Task<HashSet<string>> StartHtmlCrawler(string address)
        {
            var htmlCrawler = new HtmlCrawler(address);

            try
            {
                if (!htmlCrawler.CheckUrl(address))
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

        private async Task<HashSet<string>> StartXmlCrawler(string address)
        {
            address += "/sitemap.xml";

            var xmlCrawler = new XmlCrawler();

            return await xmlCrawler.ParseUrl(address);
        }

        private void CompareCrawlResults(HashSet<string> urlsFromHtmlCrawling, HashSet<string> urlsFromXmlCrawling)
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

        private void ShowDifferentUrls(HashSet<string> urlsFromHtmlCrawling, HashSet<string> urlsFromXmlCrawling)
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
            var timer = new Timer();

            var responseTime = await timer.GetResponseTime(urlList);

            PrintTimeResponse(responseTime);
        }

        private void PrintTimeResponse(Dictionary<string, long> sortedDict)
        {
            Console.WriteLine("\n\nList with url and response time for each page: \n");
            Console.WriteLine("\nURL".PadRight(50) + "Timing (ms)");
            
            foreach (var pair in sortedDict)
            {
                Console.WriteLine(pair.Key.PadRight(50) + pair.Value + "ms"); //print the result
            }
        }

        private void PrintNumberOfLinks(HashSet<string> urlsFromHtmlCrawling, HashSet<string> urlsFromXmlCrawling)
        {
            Console.WriteLine($"\nUrls(html documents) found after crawling a website: {urlsFromHtmlCrawling.Count}");

            Console.WriteLine($"\nUrls found in sitemap: {urlsFromXmlCrawling.Count}");
        }
    }
}
