using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml;

namespace CrawlerLogic.Crawlers
{
    public class XmlCrawler
    {
        public async Task<bool> CheckIfSiteIsHtmlDoc(string address)
        {
            var httpClient = new HttpClient();

            using var response = await httpClient.GetAsync(address, HttpCompletionOption.ResponseHeadersRead);

            return response.Content.Headers.ContentType?.MediaType == "text/html";
        }

        public async Task<HashSet<string>> ParseUrl(string address) //retrieves all urls from sitemap.xml
        {
            var urlList = new HashSet<string>();

            try
            {
                var settings = new XmlReaderSettings
                {
                    Async = true
                };

                using var reader = XmlReader.Create(address, settings);

                while (await reader.ReadAsync())
                {
                    if (reader.Name == "loc")
                    {
                        urlList = await AddUrl(reader, urlList, address);
                    }
                }

                return urlList;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }

            return urlList;
        }

        private async Task<HashSet<string>> AddUrl(XmlReader reader, HashSet<string> urlList, string address)
        {
            var innerXml = await reader.ReadInnerXmlAsync().ConfigureAwait(false);

            var absoluteUrl = GetAbsoluteUrlString(address, innerXml);

            if (await CheckIfSiteIsHtmlDoc(absoluteUrl))
            {
                urlList.Add(absoluteUrl);
            }

            return urlList;
        }

        private string GetAbsoluteUrlString(string baseUrl, string url) //gets absolute url if it is relative
        {
            var absoluteUrl = new Uri(new Uri(baseUrl), url);

            return absoluteUrl.ToString().TrimEnd('/');
        }
    }


}
