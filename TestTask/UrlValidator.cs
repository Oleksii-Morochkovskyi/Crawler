
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
            var forbiddenExtensions = new List<string> { ".txt", ".md", ".pdf", ".doc", ".docx", ".ppt", ".pptx", ".xls", ".xlsx", ".xml", ".json", ".svg", ".mp3", ".mp4" };

            return !forbiddenExtensions.Any(address.Contains);
        }
    }
}
