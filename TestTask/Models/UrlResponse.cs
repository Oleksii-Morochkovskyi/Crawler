using Crawler.Logic.Enums;

namespace Crawler.Logic.Models
{
    public class UrlResponse
    {
        public string Url { get; set; }
        public int ResponseTimeMs { get; set; }
        public Location Location { get; set; }
    }
}
