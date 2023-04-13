namespace Crawler.Persistence.Entities
{
    public class InitialUrl : BaseEntity
    {
        public InitialUrl()
        {
            FoundUrls = new List<FoundUrl>();
        }

        public string BaseUrl { get; set; }
        public DateTime DateTime { get; set; }
        public ICollection<FoundUrl> FoundUrls { get; set; }
    }
}
