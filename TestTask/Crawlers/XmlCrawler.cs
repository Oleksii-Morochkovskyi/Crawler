using System.Xml;
using Crawler.Logic.Helpers;
using Crawler.Logic.Interfaces;
using Crawler.Logic.Validators;

namespace Crawler.Logic.Crawlers
{
    public class XmlCrawler
    {
        private readonly UrlHelper _urlHelper;
        private readonly UrlValidator _validator;
        private readonly IOutputWriter _logger;

        public XmlCrawler(IOutputWriter logger, UrlHelper helper, UrlValidator validator)
        {
            _urlHelper = helper;
            _validator = validator;
            _logger = logger;
        }

        public async Task<ICollection<string>> CrawlAsync(string address)
        {
            ICollection<string> urlList = new HashSet<string>();

            try
            {
                urlList = await ExtractLinksAsync(address);
            }
            catch (Exception e)
            {
                _logger.Write(e.Message);
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

        private XmlReader CreateXmlReader(string address)
        {
            var settings = new XmlReaderSettings
            {
                Async = true
            };

            address += "/sitemap.xml";

            return XmlReader.Create(address, settings);
        }
    }
}
