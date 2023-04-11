namespace Crawler.UrlRepository.Interfaces
{
    public interface IInitialUrlRepository
    {
        Task AddInitialUrlAsync(string baseUrl);
    }
}
