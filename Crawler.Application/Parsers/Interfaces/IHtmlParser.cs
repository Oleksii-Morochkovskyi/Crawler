using HtmlAgilityPack;

namespace Crawler.Application.Parsers.Interfaces;

public interface IHtmlParser
{
    Task<ICollection<string>> ParseAsync(string baseUrl, string url);
    Task<HtmlDocument> GetHtmlAsync(string url);
    ICollection<string> ExtractLinks(HtmlNodeCollection nodes, string baseUrl);
}