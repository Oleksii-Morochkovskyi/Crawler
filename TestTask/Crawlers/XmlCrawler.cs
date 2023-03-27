using System.ComponentModel.DataAnnotations;
using System.Xml;

namespace CrawlerLogic.Crawlers
{
    public class XmlCrawler
    {
        private readonly HttpClient _httpClient;
        private readonly UrlManager _urlManager;
        private readonly UrlValidator _validator;

        public XmlCrawler(HttpClient client, string address)
        {
            _httpClient = client;
            _urlManager = new UrlManager();
            _validator = new UrlValidator(address, _httpClient);
        }

        public async Task<ICollection<string>> ParseUrlAsync(string address) //retrieves all urls from sitemap.xml
        {
            ICollection<string> urlList = new HashSet<string>();

            try
            {
                using var reader = CreateXmlReader(address);

                urlList = await ExtractLinksAsync(reader, address);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }

            return urlList;
        }

        private XmlReader CreateXmlReader(string address)
        {
            var settings = new XmlReaderSettings
            {
                Async = true
            };

            return XmlReader.Create(address, settings);
        }

        private async Task<ICollection<string>> ExtractLinksAsync(XmlReader reader, string address)
        {
            ICollection<string> urlList = new HashSet<string>();

            while (await reader.ReadAsync())
            {
                if (reader.Name == "loc")
                {
                    urlList = await AddUrlAsync(reader, urlList, address);
                }
            }

            return urlList;
        }

        private async Task<ICollection<string>> AddUrlAsync(XmlReader reader, ICollection<string> urlList, string address)
        {
            var innerXml = await reader.ReadInnerXmlAsync();

            var absoluteUrl = _urlManager.GetAbsoluteUrlString(address, innerXml);

            if (await _validator.IsHtmlDocAsync(absoluteUrl))
            {
                urlList.Add(absoluteUrl);
            }

            return urlList;
        }

    }
}
