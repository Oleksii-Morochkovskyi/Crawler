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

        public async Task<ICollection<string>> ParseAsync(string url, ICollection<string> checkedUrls, ICollection<string> urlsToCheck)
        {
            var nodes = await GetNodesAsync(url);

            var urls = ExtractLinksAsync(nodes, url, checkedUrls, urlsToCheck);

            return urls;
        }

        public async Task<HtmlNodeCollection> GetNodesAsync(string url)
        {
            var html = await GetHtmlAsync(url);

            return html.DocumentNode.SelectNodes("//a[@href]");
        }

        private async Task<HtmlDocument> GetHtmlAsync(string url) //retrieves html document from url
        {
            var html = await _httpClient.GetStringAsync(url);

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            return htmlDoc;
        }

        private ICollection<string> ExtractLinksAsync(HtmlNodeCollection nodes, string address, ICollection<string> checkedUrls, ICollection<string> urlsToCheck)
        {
            foreach (var node in nodes)
            {
                var href = node.Attributes["href"].Value;

                var absoluteUrl = _urlHelper.GetAbsoluteUrl(address, href);

                if (!urlsToCheck.Contains(absoluteUrl) && !checkedUrls.Contains(absoluteUrl) && _urlValidator.IsValidUrl(absoluteUrl) && _urlValidator.IsHtmlDocAsync(absoluteUrl))
                {
                    urlsToCheck.Add(absoluteUrl);
                }
            }

            return urlsToCheck;
        }
    }
}
