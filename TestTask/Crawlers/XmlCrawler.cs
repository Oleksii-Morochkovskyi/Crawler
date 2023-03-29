using System.Xml;
using IOManager;

namespace Crawler.Logic.Crawlers
{
    public class XmlCrawler
    {
        private readonly UrlManager _urlManager;
        private readonly UrlValidator _validator;
        private readonly XmlReader _reader;
        private readonly ILogger _logger;

        public XmlCrawler(string address)
        {
            _urlManager = new UrlManager();
            _validator = new UrlValidator(address);
            _logger = new Logger();
            
            address += "/sitemap.xml";
            _reader = CreateXmlReader(address);
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

        private XmlReader CreateXmlReader(string address)
        {
            var settings = new XmlReaderSettings
            {
                Async = true
            };

            return XmlReader.Create(address, settings);
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

            var absoluteUrl = _urlManager.GetAbsoluteUrl(address, innerXml);

            if (_validator.IsHtmlDocAsync(absoluteUrl))
            {
                urls.Add(absoluteUrl);
            }

            return urls;
        }

    }
}
