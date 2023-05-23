using Crawler.Domain.Entities;

namespace Crawler.Application.Crawlers.Interfaces;

public interface ICrawler
{
    Task<IEnumerable<UrlResponse>> StartCrawlerAsync(string address);

    IEnumerable<UrlResponse> SetUrlLocation(IEnumerable<UrlResponse> urls, ICollection<string> urlsFromHtml, ICollection<string> urlsFromXml);
}