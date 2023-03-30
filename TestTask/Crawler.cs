using Crawler.Logic.Crawlers;
using Crawler.Logic.Models;
using System.Xml;
using Crawler.Logic.Enums;
using Crawler.Logic.Interfaces;
using Crawler.Logic.TimeTracker;
using Crawler.Logic.UrlTools;

namespace Crawler.Logic
{
    public class Crawler
    {
        private readonly ILogger _logger;
        private readonly UrlHelper _helper;
        private readonly UrlValidator _validator;
        private readonly ResponseTimeTracker _tracker;
        private readonly HtmlCrawler _htmlCrawler;

        public Crawler(ILogger logger, UrlHelper helper,
                        UrlValidator validator, ResponseTimeTracker tracker, HtmlCrawler crawler)
        {
            _logger = logger;
            _helper = helper;
            _validator = validator;
            _tracker = tracker;
            _htmlCrawler = crawler;
        }

        public async Task<IList<UrlResponse>> StartCrawlerAsync(string address)
        {
            var urlsFromHtmlCrawler = await StartHtmlCrawlerAsync(address);

            var urlsFromXmlCrawler = await StartXmlCrawlerAsync(address);

            var allUrls = urlsFromHtmlCrawler.Union(urlsFromXmlCrawler);

            var urlWithResponseTime = await GetResponseTimeAsync(allUrls);

            return SetUrlLocation(urlWithResponseTime, urlsFromHtmlCrawler, urlsFromXmlCrawler);
        }

        private async Task<ICollection<string>> StartHtmlCrawlerAsync(string address)
        {
            return await _htmlCrawler.CrawlAsync(address);
        }

        private async Task<ICollection<string>> StartXmlCrawlerAsync(string address)
        {
            var reader = CreateXmlReader(address);

            var xmlCrawler = new XmlCrawler(reader, _logger, _helper, _validator);

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
            return await _tracker.GetResponseTimeAsync(urls);
        }

        private IList<UrlResponse> SetUrlLocation(IList<UrlResponse> urls, ICollection<string> urlsFromHtml, ICollection<string> urlsFromXml)
        {
            foreach (var url in urls)
            {
                url.Location = urlsFromHtml.Contains(url.Url) ? 
                    (urlsFromXml.Contains(url.Url) ? Location.Both : Location.Html) : Location.Xml;
            }

            return urls;
        }
    }
}
