namespace Crawler.Domain.Helpers
{
    public class UrlHelper
    {
        public virtual string GetAbsoluteUrl(string baseUrl, string path)
        {
            var baseUri = new Uri(baseUrl);

            var absoluteUrl = new Uri(baseUri, path);

            return absoluteUrl.ToString().TrimEnd('/');
        }
    }
}