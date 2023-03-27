namespace CrawlerLogic
{
    public class UrlValidator
    {
        private readonly string _host;
        private readonly HttpClient _httpClient;

        public UrlValidator(string address, HttpClient httpClient)
        {
            _host = new Uri(address).Host;
            _httpClient = httpClient;
        }

        public bool IsValidUrl(string address)     //Checks if url corresponds criteria  - "contains html document"
        {
            return address.Contains("http") && !address.Contains('#') && address.Contains(_host);
        }

        public async Task<bool> IsHtmlDocAsync(string address)
        {
            using var response = await _httpClient.GetAsync(address, HttpCompletionOption.ResponseHeadersRead);

            return response.Content.Headers.ContentType?.MediaType == "text/html";
        }
    }
}
