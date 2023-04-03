namespace Crawler.Logic.Validators
{
    public class UrlValidator
    {
        public bool IsValidUrl(string address)
        {
            return Uri.TryCreate(address, UriKind.Absolute, out Uri uri);
        }

        public bool IsCorrectFormat(string address, string baseUrl)
        {
            var host = new Uri(baseUrl).Host;

            return address.Contains("http") && !address.Contains('#') && address.Contains(host);
        }

        public bool IsHtmlDoc(string address, string baseUrl)
        {
            var possibleExtensions = new List<string> { ".html", ".htm", ".php", ".asp", ".aspx" };

            var path = address.Remove(0, baseUrl.Length);

            var isNotHtmlDoc = path.Contains('.') && !possibleExtensions.Any(x => path.Contains(x));
            
            return !isNotHtmlDoc;
        }

        
    }
}
