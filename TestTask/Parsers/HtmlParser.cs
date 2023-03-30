using Crawler.Logic.UrlTools;
using HtmlAgilityPack;

namespace Crawler.Logic.Parsers
{
    public class HtmlParser
    {
        private readonly HttpClient _httpClient;
        private readonly UrlValidator _urlValidator;
        private readonly UrlHelper _urlHelper;

        public HtmlParser(HttpClient httpClient, UrlHelper helper, UrlValidator validator)
        {
            _httpClient = httpClient;
            _urlValidator = validator;
            _urlHelper = helper;
        }

        public async Task<ICollection<string>> ParseAsync(string baseUrl, string url, ICollection<string> checkedUrls, ICollection<string> urlsToCheck)
        {
            var nodes = await GetNodesAsync(url);

            return ExtractLinks(nodes, baseUrl, checkedUrls, urlsToCheck);
        }

        public async Task<HtmlNodeCollection> GetNodesAsync(string url)
        {
            var html = await GetHtmlAsync(url);

            return html.DocumentNode.SelectNodes("//a[@href]");
        }

        private async Task<HtmlDocument> GetHtmlAsync(string url)
        {
            var html = await _httpClient.GetStringAsync(url);

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            return htmlDoc;
        }

        private ICollection<string> ExtractLinks(HtmlNodeCollection nodes, string baseUrl, ICollection<string> checkedUrls, ICollection<string> urlsToCheck)
        {
            foreach (var node in nodes)
            {
                var href = node.Attributes["href"].Value;

                var absoluteUrl = _urlHelper.GetAbsoluteUrl(baseUrl, href);

                if (_urlValidator.IsUrlUncheckedValidAndHtmlDoc(checkedUrls, urlsToCheck, absoluteUrl, baseUrl))
                {
                    urlsToCheck.Add(absoluteUrl);
                }
            }

            return urlsToCheck;
        }
    }
}
