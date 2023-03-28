using HtmlAgilityPack;

namespace CrawlerLogic.Parsers
{
    public class HtmlParser
    {
        private readonly HttpClient _httpClient;

        public HtmlParser(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ICollection<string>> ParseUrlAsync(string address)
        {
            var nodes = await GetNodesAsync(address);

            var urls = await ExtractLinksAsync(nodes, address);

            return urls;
        }

        public async Task<HtmlNodeCollection> GetNodesAsync(string address)
        {
            var html = await GetHtmlAsync(address);

            return html.DocumentNode.SelectNodes("//a[@href]");
        }

        private async Task<HtmlDocument> GetHtmlAsync(string address) //retrieves html document from url
        {
            var html = await _httpClient.GetStringAsync(address);

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            return htmlDoc;
        }

        private async Task<ICollection<string>> ExtractLinksAsync(HtmlNodeCollection nodes, string address)
        {
            ICollection<string> urls = new HashSet<string>();

            var validator = new UrlValidator(address,_httpClient);
            var urlManager = new UrlManager();

            foreach (var node in nodes)
            {
                var href = node.Attributes["href"].Value;

                var absoluteUrl = urlManager.GetAbsoluteUrlString(address, href);

                if (validator.IsValidUrl(absoluteUrl) && await validator.IsHtmlDocAsync(absoluteUrl))
                {
                   urls.Add(absoluteUrl);
                }
            }

            return urls;
        }
    }
}
