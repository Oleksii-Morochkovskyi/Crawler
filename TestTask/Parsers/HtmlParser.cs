using Crawler.Logic.Helpers;
using HtmlAgilityPack;
using Crawler.Logic.Interfaces;

namespace Crawler.Logic.Parsers
{
    public class HtmlParser
    {
        private readonly IHttpClient _httpClient;
        private readonly UrlHelper _urlHelper;

        public HtmlParser(IHttpClient httpClient, UrlHelper helper)
        {
            _httpClient = httpClient;
            _urlHelper = helper;
        }

        public async Task<ICollection<string>> ParseAsync(string baseUrl, string url)
        {
            var html = await GetHtmlAsync(url);

            var nodes = html.DocumentNode.SelectNodes("//a[@href]");

            return ExtractLinks(nodes, baseUrl);
        }

        private async Task<HtmlDocument> GetHtmlAsync(string url)
        {
            var html = await _httpClient.GetStringAsync(url);

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            return htmlDoc;
        }

        private ICollection<string> ExtractLinks(HtmlNodeCollection nodes, string baseUrl)
        {
            ICollection<string> parsedUrls = new HashSet<string>();
            
            foreach (var node in nodes)
            {
                var href = node.Attributes["href"].Value;

                var absoluteUrl = _urlHelper.GetAbsoluteUrl(baseUrl, href);

                parsedUrls.Add(absoluteUrl);
            }

            return parsedUrls;
        }
    }
}
