using Crawler.Domain.Enums;
using Crawler.Domain.Entities;
using Crawler.Utils.ViewModels;

namespace Crawler.Utils.Mappers
{
    public class ModelMapper
    {
        public ResultViewModel GetResultViewModel(IEnumerable<FoundUrl> foundUrls)
        {
            var foundUrlsViewModel = MapFoundUrls(foundUrls);

            var xmlExceptHtml = foundUrlsViewModel.Where(x => x.Location == Location.Xml)
                .Select(x => x.Url);

            var htmlExceptXml = foundUrlsViewModel.Where(x => x.Location == Location.Html)
                .Select(x => x.Url);

            return new ResultViewModel
            {
                FoundUrls = foundUrlsViewModel,
                UrlsFromHtml = htmlExceptXml,
                UrlsFromXml = xmlExceptHtml
            };
        }

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

        private IEnumerable<FoundUrlViewModel> MapFoundUrls(IEnumerable<FoundUrl> foundUrls)
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
