using System.Net;
using System.Runtime.InteropServices.ComTypes;
using CrawlerLogic.Crawlers;
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
    }
}
