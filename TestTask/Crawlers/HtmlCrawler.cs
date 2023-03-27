using HtmlAgilityPack;
using System.Net;
using System.Xml.Linq;

namespace CrawlerLogic.Crawlers
{
    public class HtmlCrawler
    {
        //private string _address { get; }

        private HashSet<string> _urlList;
        private HashSet<string> _checkedUrlList;
        private readonly UrlManager _urlManager;
        private readonly HttpClient _httpClient;

        public HtmlCrawler(string address, HttpClient client)
        {
            _httpClient = client;
            _urlManager = new UrlManager(address, _httpClient);
            _urlList = new HashSet<string>();
            _checkedUrlList = new HashSet<string>();

        }

        public async Task<ICollection<string>> ParseUrl(string address) //method for crawling page and finding urls on it
        {
            _checkedUrlList.Add(address);

            var nodes = await GetNodes(address);

            await ExtractLinks(nodes, address);

            return _urlList;
        }

        private async Task ExtractLinks(HtmlNodeCollection nodes, string address)
        {
            foreach (var node in nodes)
            {
                var href = node.Attributes["href"].Value;

                try
                {
                    var absoluteUrl = _urlManager.GetAbsoluteUrlString(address, href);

                    await AddUrl(absoluteUrl);
                }
                catch (Exception)
                {
                    Console.WriteLine($"Can't open url {_urlManager.GetAbsoluteUrlString(address, href)}");
                }
            }
        }

        private async Task AddUrl(string absoluteUrl)
        {
            if (_urlList.Contains(absoluteUrl) || !_urlManager.CheckUrl(absoluteUrl) || !await _urlManager.CheckIfSiteIsHtmlDoc(absoluteUrl))
            {
                _checkedUrlList.Add(absoluteUrl);
                return;
            }

            _urlList.Add(absoluteUrl);

            if (!_checkedUrlList.Contains(absoluteUrl))
            {
                await ParseUrl(absoluteUrl);
            }
        }

        private async Task<HtmlNodeCollection> GetNodes(string address)
        {
            var html = await _urlManager.GetHtml(address);

            return html.DocumentNode.SelectNodes("//a[@href]");
        }
    }
}
