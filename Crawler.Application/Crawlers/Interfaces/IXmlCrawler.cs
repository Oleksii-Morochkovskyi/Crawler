using System.Xml;

namespace Crawler.Application.Crawlers.Interfaces;

public interface IXmlCrawler
{
    Task<ICollection<string>> CrawlAsync(string address);
    Task<XmlNodeList> GetXmlLocNodes(string address);
    ICollection<string> ExtractLinksAsync(XmlNodeList nodes, string baseUrl);
}