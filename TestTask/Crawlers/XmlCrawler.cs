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
        private readonly XmlReader _reader;
        private readonly ILogger _logger;

        public XmlCrawler(XmlReader reader, ILogger logger, UrlHelper helper, UrlValidator validator)
        {
            _urlHelper = helper;
            _validator = validator;
            _logger = logger;
            _reader = reader;
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
            ICollection<string> urls = new HashSet<string>();

            while (await _reader.ReadAsync())
            {
                if (_reader.Name == "loc")
                {
                    urls = await AddUrlAsync(urls, address);
                }
            }

            return urls;
        }

        private async Task<ICollection<string>> AddUrlAsync(ICollection<string> urls, string address)
        {
            var innerXml = await _reader.ReadInnerXmlAsync();

            var absoluteUrl = _urlHelper.GetAbsoluteUrl(address, innerXml);

            if (_validator.IsHtmlDoc(absoluteUrl, address))
            {
                urls.Add(absoluteUrl);
            }

            return urls;
        }
    }
}
