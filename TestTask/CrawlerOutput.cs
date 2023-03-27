using CrawlerLogic.Models;

namespace CrawlerLogic
{
    public class CrawlerOutput
    {
        public void PrintList(IEnumerable<string> urls)
        {
            foreach (var url in urls)
            {
                Console.WriteLine(url);
            }
        }

        public void PrintTimeResponse(IList<ResponseTime> urls)
        {
            Console.WriteLine("\n\nList with url and response time for each page: \n");
            Console.WriteLine("\nURL".PadRight(50) + "Timing (ms)\n");

            foreach (var url in urls)
            {
                Console.WriteLine(url._url.PadRight(50) + url._responseTime + "ms"); //print the result
            }
        }

        public void PrintNumberOfLinks(ICollection<string> urlsFromHtmlCrawling, ICollection<string> urlsFromXmlCrawling)
        {
            Console.WriteLine($"\nUrls(html documents) found after crawling a website: {urlsFromHtmlCrawling.Count}");

            Console.WriteLine($"\nUrls found in sitemap: {urlsFromXmlCrawling.Count}");
        }
    }
}
