namespace Crawler.Logic.UrlTools
{
    public class UrlValidator
    {
        public bool IsInputUrlCorrect(string address)
        {
            return Uri.TryCreate(address, UriKind.Absolute, out Uri uri);
        }

        public bool IsValidUrl(string address, string baseUrl)
        {
            var host = new Uri(baseUrl).Host;

            return address.Contains("http") && !address.Contains('#') && address.Contains(host);
        }

        public bool IsHtmlDoc(string address, string baseUrl)
        {
            var possibleExtensions = new List<string> { ".html", ".htm", ".php", ".asp", ".aspx" };

            var path = address.Remove(0, baseUrl.Length);

            return !(path.Contains('.') && !possibleExtensions.Any(path.Contains));
        }

        public bool IsUrlUncheckedValidAndHtmlDoc(ICollection<string> checkedUrls, ICollection<string> urlsToCheck, string url, string baseUrl)
        {
            return !urlsToCheck.Contains(url) && !checkedUrls.Contains(url) && IsValidUrl(url, baseUrl)
                   && IsHtmlDoc(url, baseUrl);
        }
    }
}
