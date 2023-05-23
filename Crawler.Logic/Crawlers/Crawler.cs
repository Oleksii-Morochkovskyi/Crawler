using Crawler.Domain.Entities;
using Crawler.Domain.Enums;
using Crawler.Application.Crawlers.Interfaces;
using Crawler.Application.Services.Interfaces;

namespace Crawler.Logic.Crawlers
{
    public class Crawler : ICrawler
    {
        private readonly IResponseTimeService _responseTimeService;
        private readonly IHtmlCrawler _htmlCrawler;
        private readonly IXmlCrawler _xmlCrawler;

        public Crawler(
            IResponseTimeService responseTimeService,
            IHtmlCrawler htmlCrawler,
            IXmlCrawler xmlCrawler)
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

        public IEnumerable<UrlResponse> SetUrlLocation(IEnumerable<UrlResponse> urls, ICollection<string> urlsFromHtml,
            ICollection<string> urlsFromXml)
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