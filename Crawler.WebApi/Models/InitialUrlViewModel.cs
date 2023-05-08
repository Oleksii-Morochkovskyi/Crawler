using Crawler.Persistence.Entities;

namespace Crawler.WebApi.Models
{
    public class InitialUrlViewModel
    {
        public int Id { get; set; }
        public string BaseUrl { get; set; }
        public DateTime DateTime { get; set; }

        public IEnumerable<InitialUrlViewModel> MapInitialUrls(IEnumerable<InitialUrl> initialUrls)
        {
            var urls = initialUrls.Select(url => new InitialUrlViewModel()
            {
                Id = url.Id,
                BaseUrl = url.BaseUrl,
                DateTime = url.DateTime,
            });

            return urls;
        }
    }
}
