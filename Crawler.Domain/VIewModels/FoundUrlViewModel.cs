using Crawler.Domain.Enums;

namespace Crawler.Domain.ViewModels
{
    public class FoundUrlViewModel
    {
        public string Url { get; set; }
        public int ResponseTimeMs { get; set; }
        public Location Location { get; set; }
        public int InitialUrlId { get; set; }
    }
}
