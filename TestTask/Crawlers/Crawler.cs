﻿using Crawler.Logic.Models;
using Crawler.Logic.Enums;
using Crawler.Logic.Services;

namespace Crawler.Logic.Crawlers
{
    public class Crawler
    {
        private readonly ResponseTimeService _service;
        private readonly HtmlCrawler _htmlCrawler;
        private readonly XmlCrawler _xmlCrawler;

        public Crawler(ResponseTimeService service, HtmlCrawler htmlCrawler, XmlCrawler xmlCrawler)
        {
            _service = service;
            _htmlCrawler = htmlCrawler;
            _xmlCrawler = xmlCrawler;
        }

        public virtual async Task<IEnumerable<UrlResponse>> StartCrawlerAsync(string address)
        {
            var urlsFromHtmlCrawler = await _htmlCrawler.CrawlAsync(address);

            var urlsFromXmlCrawler = await _xmlCrawler.CrawlAsync(address);

            var allUrls = urlsFromHtmlCrawler.Union(urlsFromXmlCrawler);

            var urlWithResponseTime = await _service.GetResponseTimeAsync(allUrls);

            return SetUrlLocation(urlWithResponseTime, urlsFromHtmlCrawler, urlsFromXmlCrawler);
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
