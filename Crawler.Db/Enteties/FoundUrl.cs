using Crawler.Logic.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Crawler.Db.Enteties
{
    public class FoundUrl
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public int ResponseTimeMs { get; set; }
        public Location Location { get; set; }
        public int DomainUrlId { get; set; }
        public DomainUrl DomainUrl { get; set; }
    }
}
