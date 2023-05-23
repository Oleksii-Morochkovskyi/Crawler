using Crawler.Domain.Entities;

namespace Crawler.Application.Services.Interfaces;

public interface IResponseTimeService
{
    Task<IEnumerable<UrlResponse>> GetResponseTimeAsync(IEnumerable<string> urls);
    Task<int> CalculateTimeAsync(string url);
}