namespace Crawler.Application.Crawlers.Interfaces;

public interface IHtmlCrawler
{
    Task<ICollection<string>> CrawlAsync(string baseUrl);

    ICollection<string> FilterLinks(string baseUrl, ICollection<string> checkedUrls, ICollection<string> urlsToCheck, ICollection<string> parsedUrls);
}