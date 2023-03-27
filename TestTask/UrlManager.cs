using HtmlAgilityPack;

namespace CrawlerLogic
{
    public class UrlManager
    {
        private readonly string _host;
        private readonly HttpClient _httpClient;

        public UrlManager(string address, HttpClient httpClient)
        {
            _host = new Uri(address).Host;
            _httpClient = httpClient;
        }

        public string GetAbsoluteUrlString(string baseUrl, string url) //gets absolute url if it is relative
        {
            var absoluteUrl = new Uri(new Uri(baseUrl), url);

            return absoluteUrl.ToString().TrimEnd('/');
        }

        public bool CheckUrl(string address)     //Checks if url corresponds criteria  - "contains html document"
        {
            return address.Contains("http") && !address.Contains('#') && address.Contains(_host);
        }

        public async Task<bool> CheckIfSiteIsHtmlDoc(string address)
        {
            using var response = await _httpClient.GetAsync(address, HttpCompletionOption.ResponseHeadersRead);

            return response.Content.Headers.ContentType?.MediaType == "text/html";
        }

        public async Task<HtmlDocument> GetHtml(string address) //retrieves html document from url
        {
            var html = await _httpClient.GetStringAsync(address);

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            return htmlDoc;
        }
    }
}