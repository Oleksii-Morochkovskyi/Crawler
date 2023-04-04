using System.Xml;
using Crawler.Logic.Helpers;
using Crawler.Logic.Interfaces;
using Crawler.Logic.Services;
using Crawler.Logic.Validators;

namespace Crawler.Logic.Crawlers
{
    public class XmlCrawler
    {
        private readonly UrlHelper _urlHelper;
        private readonly UrlValidator _validator;
        private readonly IConsoleHandler _consoleHandler;
        private readonly HttpClientService _httpClientService;

        public XmlCrawler(IConsoleHandler consoleHandler, UrlHelper helper, UrlValidator validator, HttpClientService httpClientService)
        {
            _urlHelper = helper;
            _validator = validator;
            _consoleHandler = consoleHandler;
            _httpClientService = httpClientService;
        }

        public virtual async Task<ICollection<string>> CrawlAsync(string address)
        {
            ICollection<string> urlList = new HashSet<string>();
            var sitemapUrl = address + "/sitemap.xml";

            try
            {
                var locNodes = await GetXmlLocNodes(sitemapUrl);

                urlList = ExtractLinksAsync(locNodes, address);
            }
            catch (Exception e)
            {
                _consoleHandler.Write(e.Message);
            }

            return urlList;
        }

        private async Task<XmlNodeList> GetXmlLocNodes(string address)
        {
            var xmlDoc = new XmlDocument();

            var xmlString = await _httpClientService.GetStringAsync(address);

            xmlDoc.LoadXml(xmlString);

            return xmlDoc.GetElementsByTagName("loc");
        }

        private ICollection<string> ExtractLinksAsync(XmlNodeList nodes, string baseUrl)
        {
            ICollection<string> urlList = new HashSet<string>();

            foreach (XmlNode node in nodes)
            {
                var url = node.InnerText;

                var absoluteUrl = _urlHelper.GetAbsoluteUrl(baseUrl, url);

                if (_validator.IsHtmlDoc(absoluteUrl, baseUrl))
                {
                    urlList.Add(absoluteUrl);
                }
            }

            return urlList;
        }
        /*public async Task<ICollection<string>> CrawlAsync(string address)
        {
            ICollection<string> urlList = new HashSet<string>();

            try
            {
                urlList = await ExtractLinksAsync(address);
            }
            catch (Exception e)
            {
                _consoleHandler.Write(e.Message);
            }

            return urlList;
        }

        private async Task<ICollection<string>> ExtractLinksAsync(string address)
        {
            var reader = CreateXmlReader(address);

            ICollection<string> urls = new HashSet<string>();

            while (await reader.ReadAsync())
            {
                if (reader.Name == "loc")
                {
                    urls = await AddUrlAsync(urls, address, reader);
                }
            }

            return urls;
        }

        private async Task<ICollection<string>> AddUrlAsync(ICollection<string> urls, string address, XmlReader reader)
        {
            var innerXml = await reader.ReadInnerXmlAsync();

            var absoluteUrl = _urlHelper.GetAbsoluteUrl(address, innerXml);

            if (_validator.IsHtmlDoc(absoluteUrl, address))
            {
                urls.Add(absoluteUrl);
            }

            return urls;
        }

        public virtual XmlReader CreateXmlReader(string address)
        {
            var settings = new XmlReaderSettings
            {
                Async = true
            };

            address += "/sitemap.xml";

            return XmlReader.Create(address, settings);
        }*/
    }
}
