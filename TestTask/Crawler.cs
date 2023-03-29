using Crawler.Logic.Crawlers;
using Crawler.Logic.Interfaces;
using Crawler.Logic.Models;
using Crawler.Logic.Parsers;
using System.Linq;
using System.Xml;
using Crawler.Logic.Enums;

namespace Crawler.Logic
{
    public class Crawler
    {
        private readonly HttpClient _httpClient;
        private readonly Logger _logger;
        private readonly UrlHelper _helper;
        private readonly UrlValidator _validator;

        public Crawler(HttpClient client, Logger logger, UrlHelper helper, UrlValidator validator)
        {
            _httpClient = client;
            _logger = logger;
            _helper = helper;
            _validator = validator;
        }

        public async Task<IList<UrlResponse>> StartCrawlerAsync(string address)
        {
            var urlsFromHtmlCrawler = await StartHtmlCrawlerAsync(address);

            var urlsFromXmlCrawler = await StartXmlCrawlerAsync(address);

            var allUrls = urlsFromHtmlCrawler.Union(urlsFromXmlCrawler);

            var responseTime = await GetResponseTimeAsync(allUrls);

            return FillResultSet(responseTime,urlsFromHtmlCrawler,urlsFromXmlCrawler);
        }

        private async Task<ICollection<string>> StartHtmlCrawlerAsync(string address)
        {
            var htmlParser = new HtmlParser(_httpClient, _helper, _validator);
            var htmlCrawler = new HtmlCrawler(_logger, htmlParser);

            return await htmlCrawler.CrawlAsync(address);
        }

        private async Task<ICollection<string>> StartXmlCrawlerAsync(string address)
        {
            var reader = CreateXmlReader(address);

            var xmlCrawler = new XmlCrawler(reader, _logger, _helper,_validator);

            return await xmlCrawler.CrawlAsync(address);
        }

        private XmlReader CreateXmlReader(string address)
        {
            var settings = new XmlReaderSettings
            {
                Async = true
            };

            address += "/sitemap.xml";

            return XmlReader.Create(address, settings);
        }

        private async Task<IList<UrlResponse>> GetResponseTimeAsync(IEnumerable<string> urls)
        {
            var tracker = new ResponseTimeTracker(_httpClient, _logger);

            return await tracker.GetResponseTimeAsync(urls);
        }

        private IList<UrlResponse> FillResultSet(IList<UrlResponse> urls, ICollection<string> urlsFromHtml, ICollection<string> urlsFromXml)
        {
            foreach (var url in urls)
            {
                if (urlsFromHtml.Contains(url.Url))
                {
                    url.location = urlsFromXml.Contains(url.Url) ? Location.HtmlAndXml : Location.Html;
                }
                else
                {
                    url.location = Location.Xml;
                }
            }

            return urls;
        }
    }
}
