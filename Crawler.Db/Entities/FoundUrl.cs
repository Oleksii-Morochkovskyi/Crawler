using Crawler.Logic.Enums;

namespace Crawler.Persistence.Entities
{
    public class FoundUrl : BaseEntity
    {
        public string Url { get; set; }
        public int ResponseTimeMs { get; set; }
        public Location Location { get; set; }
        public int InitialUrlId { get; set; }
        public InitialUrl InitialUrl { get; set; }
    }
}
