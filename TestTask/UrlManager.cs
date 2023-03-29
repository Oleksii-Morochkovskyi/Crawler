
namespace Crawler.Logic
{
    public class UrlManager
    {
        public string GetAbsoluteUrl(string baseUrl, string path) //gets absolute url if it is relative
        {
            var baseUri = new Uri(baseUrl);

            var absoluteUrl = new Uri(baseUri, path);

            return absoluteUrl.ToString().TrimEnd('/');
        }
    }
}