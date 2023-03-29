﻿using Crawler.Logic.Interfaces;
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

        public async Task<ICollection<string>> ParseAsync(string url)
        {
            var nodes = await GetNodesAsync(url);

            var urls = ExtractLinksAsync(nodes, url);

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

        private ICollection<string> ExtractLinksAsync(HtmlNodeCollection nodes, string address)
        {
            ICollection<string> urls = new HashSet<string>();

            foreach (var node in nodes)
            {
                var href = node.Attributes["href"].Value;

                var absoluteUrl = _urlHelper.GetAbsoluteUrl(address, href);

                if (_urlValidator.IsValidUrl(absoluteUrl) && _urlValidator.IsHtmlDocAsync(absoluteUrl))
                {
                   urls.Add(absoluteUrl);
                }
            }

            return urls;
        }
    }
}
