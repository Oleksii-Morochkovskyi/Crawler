namespace Crawler.UrlRepository.Entities
{
    public class InitialUrl : BaseEntity
    {
        public InitialUrl()
        {
            FoundUrls = new List<FoundUrl>();
        }

        public string BaseUrl { get; set; }

        public ICollection<FoundUrl> FoundUrls { get; set; }
    }
}
