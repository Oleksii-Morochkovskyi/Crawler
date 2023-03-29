
namespace Crawler.Logic.Models
{
    public class ResultSet
    {
        public ICollection<string> urlsFromHtml;
        public ICollection<string> urlsFromXml;
        public ICollection<string> htmlExceptXml;
        public ICollection<string> xmlExceptHtml;
        public IList<UrlResponse> urlResponses;
    }
}
