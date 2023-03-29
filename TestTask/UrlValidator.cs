
namespace Crawler.Logic
{
    public class UrlValidator
    {
        private readonly string _host;

        public UrlValidator(string address)
        {
            _host = new Uri(address).Host;
        }

        public UrlValidator() { }

        public bool IsCorrectInput(string address)
        {
            return Uri.TryCreate(address, UriKind.Absolute, out Uri uri);
        }

        public bool IsValidUrl(string address)
        {
            return address.Contains("http") && !address.Contains('#') && address.Contains(_host);
        }

        public bool IsHtmlDocAsync(string address)
        {
            var possibleExtensions = new List<string> { ".html", ".htm", ".php", ".asp", ".aspx", "" };

            var extension = Path.GetExtension(address).ToLower();
            
            return possibleExtensions.Contains(extension);
        }
    }
}
