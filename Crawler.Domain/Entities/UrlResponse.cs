using Crawler.Domain.Enums;

namespace Crawler.Domain.Entities
{
    public class UrlResponse
    {
        public string Url { get; set; }
        public int ResponseTimeMs { get; set; }
        public Location Location { get; set; }
    }
}
