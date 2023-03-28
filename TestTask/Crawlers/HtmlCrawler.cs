using CrawlerLogic.Parsers;
using HtmlAgilityPack;

namespace CrawlerLogic.Crawlers
{
    public class HtmlCrawler
    {
        private HashSet<string> _urlList;
        private HashSet<string> _checkedUrlList;

        private readonly UrlManager _urlManager;
        private readonly UrlValidator _validator;
        private readonly HttpClient _httpClient;
        private readonly HtmlParser _parser;

        public HtmlCrawler(string address, HttpClient client)
        {
            _httpClient = client;

            _urlManager = new UrlManager();
            _validator = new UrlValidator(address, _httpClient);
            _parser = new HtmlParser(_httpClient);

            _urlList = new HashSet<string>();
            _checkedUrlList = new HashSet<string>();
        }

        public async Task<ICollection<string>> CrawlUrlAsync(string address) 
        {
            _checkedUrlList.Add(address);

            var nodes = await _parser.GetNodesAsync(address);

            await ExtractLinksAsync(nodes, address);

            return _urlList;
        }

        private async Task ExtractLinksAsync(HtmlNodeCollection nodes, string address)
        {
            foreach (var node in nodes)
            {
                var href = node.Attributes["href"].Value;

                try
                {
                    var absoluteUrl = _urlManager.GetAbsoluteUrlString(address, href);

                    await AddUrlAsync(absoluteUrl);
                }
                catch (Exception)
                {
                   // Console.WriteLine($"Can't open url {_urlManager.GetAbsoluteUrlString(address, href)}");
                }
            }
        }

        private async Task AddUrlAsync(string absoluteUrl)
        {
            if (_urlList.Contains(absoluteUrl) || !_validator.IsValidUrl(absoluteUrl) || !await _validator.IsHtmlDocAsync(absoluteUrl))
            {
                _checkedUrlList.Add(absoluteUrl);
                return;
            }

            _urlList.Add(absoluteUrl);

            if (!_checkedUrlList.Contains(absoluteUrl))
            {
                await CrawlUrlAsync(absoluteUrl);
            }
        }
    }
}
