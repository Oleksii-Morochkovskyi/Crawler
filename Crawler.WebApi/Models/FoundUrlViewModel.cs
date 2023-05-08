using Crawler.Logic.Enums;
using Crawler.Persistence.Entities;

namespace Crawler.WebApi.Models
{
    public class FoundUrlViewModel
    {
        public string Url { get; set; }
        public int ResponseTimeMs { get; set; }
        public Location Location { get; set; }
        public int InitialUrlId { get; set; }

        public IEnumerable<FoundUrlViewModel> MapFoundUrls(IEnumerable<FoundUrl> foundUrls)
        {
            var urls = foundUrls.Select(url => new FoundUrlViewModel
            {
                Url = url.Url,
                Location = url.Location,
                ResponseTimeMs = url.ResponseTimeMs,
                InitialUrlId = url.InitialUrlId
            });

            return urls;
        }
    }
}
