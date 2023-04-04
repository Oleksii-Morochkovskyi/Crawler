using Crawler.Logic.Models;
using Crawler.Logic.Enums;
using Crawler.Logic.Services;

namespace Crawler.Logic.Crawlers
{
    public class Crawler
    {
        private readonly ResponseTimeService _responseTimeService;
        private readonly HtmlCrawler _htmlCrawler;
        private readonly XmlCrawler _xmlCrawler;

        public Crawler(ResponseTimeService responseTimeService, HtmlCrawler htmlCrawler, XmlCrawler xmlCrawler)
        {
            _responseTimeService = responseTimeService;
            _htmlCrawler = htmlCrawler;
            _xmlCrawler = xmlCrawler;
        }

        public virtual async Task<IEnumerable<UrlResponse>> StartCrawlerAsync(string address)
        {
            var urlsFromHtmlCrawler = await _htmlCrawler.CrawlAsync(address);

            var urlsFromXmlCrawler = await _xmlCrawler.CrawlAsync(address);

            var allUrls = urlsFromHtmlCrawler.Union(urlsFromXmlCrawler);

            var urlWithResponseTime = await _responseTimeService.GetResponseTimeAsync(allUrls);
            
            var sortedResponseTimeList = urlWithResponseTime.OrderBy(x => x.ResponseTimeMs);

            return SetUrlLocation(sortedResponseTimeList, urlsFromHtmlCrawler, urlsFromXmlCrawler);
        }

        private IEnumerable<UrlResponse> SetUrlLocation(IEnumerable<UrlResponse> urls, ICollection<string> urlsFromHtml, ICollection<string> urlsFromXml)
        {
            foreach (var url in urls)
            {
                if (urlsFromHtml.Contains(url.Url))
                {
                    url.Location = urlsFromXml.Contains(url.Url) ? Location.Both : Location.Html;
                    continue;
                }

                url.Location = Location.Xml;
            }

            return urls;
        }
    }
}
