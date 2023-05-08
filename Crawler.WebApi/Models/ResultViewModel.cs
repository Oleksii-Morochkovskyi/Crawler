using Crawler.Logic.Enums;

namespace Crawler.WebApi.Models
{
    public class ResultViewModel
    {
        public IEnumerable<FoundUrlViewModel> FoundUrls { get; set; }
        public IEnumerable<string> UrlsFromHtml { get; set; }
        public IEnumerable<string> UrlsFromXml { get; set; }

        public ResultViewModel GetResultViewModel(IEnumerable<FoundUrlViewModel> viewModel)
        {
            var xmlExceptHtml = viewModel.Where(x => x.Location == Location.Xml)
                .Select(x => x.Url);

            var htmlExceptXml = viewModel.Where(x => x.Location == Location.Html)
                .Select(x => x.Url);

            return new ResultViewModel
            {
                FoundUrls = viewModel,
                UrlsFromHtml = htmlExceptXml,
                UrlsFromXml = xmlExceptHtml
            };
        }
    }
}
