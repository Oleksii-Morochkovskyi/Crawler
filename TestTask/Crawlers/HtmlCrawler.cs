using HtmlAgilityPack;

namespace CrawlerLogic.Crawlers
{
    public class HtmlCrawler
    {
        //private string _address { get; }
        private string _host { get; }
        //private readonly IHttpClientFactory _httpClientFactory;

        private HashSet<string> _urlList;
        private HashSet<string> _checkedUrlList;

        private readonly HttpClient _httpClient;

        public HtmlCrawler(string address)
        {
            _httpClient = new HttpClient();

            //_address = address;

            _urlList = new HashSet<string>();
            _checkedUrlList = new HashSet<string>();

            _host = new Uri(address).Host;
        }

        public async Task<HtmlDocument> GetHtml(string address) //retrieves html document from url
        {
            var html = await _httpClient.GetStringAsync(address);

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            return htmlDoc;
        }

        public bool CheckUrl(string address)     //Checks if url corresponds criteria  - "contains html document"
        {
            return address.Contains("http") && !address.Contains('#') && address.Contains(_host);
        }

        public async Task<bool> CheckIfSiteIsHtmlDoc(string address)
        {
            using var response = await _httpClient.GetAsync(address, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);

            return response.Content.Headers.ContentType?.MediaType == "text/html";
        }

        public async Task<HashSet<string>> ParseUrl(string address) //method for crawling page and finding urls on it
        {
            _checkedUrlList.Add(address);
            
            var nodes = await GetNodes(address);

            foreach (var node in nodes)
            {
                var href = node.Attributes["href"].Value;

                try
                {
                    var absoluteUrl = GetAbsoluteUrlString(address, href);

                    await AddUrl(absoluteUrl);

                }
                catch (Exception)
                {
                    Console.WriteLine($"Can't open url {GetAbsoluteUrlString(address, href)}");
                }
            }
            return _urlList;
        }

        private async Task<HtmlNodeCollection> GetNodes(string address)
        {
            var html = await GetHtml(address);

            return html.DocumentNode.SelectNodes("//a[@href]");
        }

        private async Task AddUrl(string absoluteUrl)
        {
            if (_urlList.Contains(absoluteUrl) || !CheckUrl(absoluteUrl) || !await CheckIfSiteIsHtmlDoc(absoluteUrl))
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

        private string GetAbsoluteUrlString(string baseUrl, string url) //gets absolute url if it is relative
        {
            var absoluteUrl = new Uri(new Uri(baseUrl), url);

            return absoluteUrl.ToString().TrimEnd('/');
        }
    }
}
