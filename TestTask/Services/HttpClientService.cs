
using System.Net.Http;

namespace Crawler.Logic.Services
{
    public class HttpClientService
    {
        //private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _httpClient;

        public HttpClientService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            //_httpClientFactory = httpClientFactory;
         //   _httpClient = _httpClientFactory.CreateClient();
        }

        public virtual async Task<string> GetStringAsync(string url)
        {
            return await _httpClient.GetStringAsync(url);
        }

        public virtual async Task<HttpResponseMessage> GetAsync(string url)
        {
            return await _httpClient.GetAsync(url);
        }

        /*public HttpClient CreateClient(string name)
        {
            return _httpClientFactory.CreateClient();

        }*/
    }
}
