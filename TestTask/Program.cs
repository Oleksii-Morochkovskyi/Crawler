
using System;

namespace TestTask
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter URL: ");
            string adress = Console.ReadLine();

            ListManager ListManager = new ListManager();
            HtmlParser Htmlcrawler = new HtmlParser(adress);
            try
            {
                Htmlcrawler.GetHtml(adress);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Ooooops, seems like you entered wrong url. {e.Message}. Please, try again.");
                return;
            }
            HashSet<string> parsedHtml = new HashSet<string>();
            HashSet<string> parsedSitemap = new HashSet<string>();

            Console.WriteLine("\n\nPlease wait. A process may take time.\n\n");
            parsedHtml = Htmlcrawler.ParseUrl(adress);

            if (adress.Last() == '/')
                adress += "sitemap.xml";
            else
                adress += "/sitemap.xml";


            XmlParser xmlcrawler = new XmlParser(adress);
            parsedSitemap = xmlcrawler.ParseUrl();
            if (parsedSitemap.Count() == 0)
            {
                Console.WriteLine("\nSite doesn't have sitemap.xml\n");
                Console.WriteLine("The list with url and response time for each page: \n");
                ListManager.GetResponseTime(parsedHtml);
                Console.WriteLine("---------------------------------------------------");

                Console.WriteLine($"Urls(html documents) found after crawling a website: {parsedHtml.Count()}");
                Console.WriteLine("---------------------------------------------------");
                Console.WriteLine($"Urls found in sitemap: {parsedSitemap.Count()}");
            }
            else
            {


                Console.WriteLine("\nUrls FOUND IN SITEMAP.XML but not founded after crawling a web site: \n");
                var res1 = ListManager.CompareResult(parsedSitemap, parsedHtml);
                ListManager.Print(res1);
                Console.WriteLine("---------------------------------------------------");

                Console.WriteLine("\nUrls FOUND BY CRAWLING THE WEBSITE but not in sitemap.xml: \n");
                var res2 = ListManager.CompareResult(parsedHtml, parsedSitemap);
                ListManager.Print(res2);
                Console.WriteLine("---------------------------------------------------");


                var res3 = ListManager.MergeUrls(parsedHtml, parsedSitemap);

                Console.WriteLine("\n\nList with url and response time for each page: \n");
                ListManager.GetResponseTime(res3);

                Console.WriteLine("---------------------------------------------------");
                Console.WriteLine($"Urls(html documents) found after crawling a website: {parsedHtml.Count()}");
                Console.WriteLine("---------------------------------------------------");
                Console.WriteLine($"Urls found in sitemap: {parsedSitemap.Count()}");
            }


        }
    }
}