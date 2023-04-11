namespace Crawler.UrlRepository.Entities
{
    public class InitialUrl : BaseEntity
    {
        public string BaseUrl { get; set; }

        public ICollection<FoundUrl> FoundUrls { get; set; }

        public InitialUrl()
        {
            FoundUrls = new List<FoundUrl>();
        }
    }
}
