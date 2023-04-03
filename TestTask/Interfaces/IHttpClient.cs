
namespace Crawler.Logic.Interfaces
{
    public interface IHttpClient
    { 
        Task<string> GetStringAsync(string url);
        Task<HttpResponseMessage> GetAsync(string url);
    }
}
