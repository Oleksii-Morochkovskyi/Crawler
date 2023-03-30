
namespace Crawler.Logic
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

        public bool IsHtmlDocAsync(string address)
        {
            var forbiddenExtensions = new List<string> { ".txt", ".md", ".pdf", ".doc", ".docx", ".ppt", ".pptx", ".xls", ".xlsx", ".xml", ".json", ".svg", ".mp3", ".mp4" };

            return !forbiddenExtensions.Any(address.Contains);
        }
    }
}
