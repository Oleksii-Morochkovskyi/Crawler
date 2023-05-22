using Crawler.Logic.Enums;
using Crawler.Persistence.Entities;

namespace Crawler.Services.Models
{
    public class FoundUrlViewModel
    {
        public string Url { get; set; }
        public int ResponseTimeMs { get; set; }
        public Location Location { get; set; }
        public int InitialUrlId { get; set; }
    }
}
