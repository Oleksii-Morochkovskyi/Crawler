using System.Xml;
using Crawler.Logic.Helpers;
using Crawler.Application.Services;
using Crawler.Logic.Validators;

namespace Crawler.Logic.Crawlers
{
    public class XmlCrawler
    {
        private readonly UrlHelper _urlHelper;
        private readonly UrlValidator _validator;
        private readonly HttpClientService _httpClientService;

        public XmlCrawler(UrlHelper helper,
            UrlValidator validator,
            HttpClientService httpClientService)
        {
            _urlHelper = helper;
            _validator = validator;
            _httpClientService = httpClientService;
        }

        public virtual async Task<ICollection<string>> CrawlAsync(string address)
        {
            ICollection<string> urlList = new HashSet<string>();
            var sitemapUrl = address + "/sitemap.xml";
            
            var locNodes = await GetXmlLocNodes(sitemapUrl);

            urlList = ExtractLinksAsync(locNodes, address);

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
    }
}