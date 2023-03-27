using CrawlerLogic;

namespace CrawlerManager
{
    public class CrawlerOutput
    {
        public void PrintList(IEnumerable<string> urlList)
        {
            foreach (var url in urlList)
            {
                Console.WriteLine(url);
            }
        }

        public void PrintTimeResponse(ResponseTimeModel model)
        {
            Console.WriteLine("\n\nList with url and response time for each page: \n");
            Console.WriteLine("\nURL".PadRight(50) + "Timing (ms)");

            foreach (var pair in model.ResponseTime)
            {
                Console.WriteLine(pair.Key.PadRight(50) + pair.Value + "ms"); //print the result
            }
        }

        public void PrintNumberOfLinks(ICollection<string> urlsFromHtmlCrawling, ICollection<string> urlsFromXmlCrawling)
        {
            Console.WriteLine($"\nUrls(html documents) found after crawling a website: {urlsFromHtmlCrawling.Count}");

            Console.WriteLine($"\nUrls found in sitemap: {urlsFromXmlCrawling.Count}");
        }
    }
}
