using System.Diagnostics;
using Crawler.Application.Services;
using Crawler.Application.Wrappers;
using Crawler.Domain.Entities;

namespace Crawler.Logic.Services
{
    public class ResponseTimeService
    {
        private readonly HttpClientService _httpClientService;
        private readonly ConsoleWrapper _consoleWrapper;

        public ResponseTimeService(
            HttpClientService clientService,
            ConsoleWrapper consoleWrapper)
        {
            _httpClientService = clientService;
            _consoleWrapper = consoleWrapper;
        }

        public virtual async Task<IEnumerable<UrlResponse>> GetResponseTimeAsync(IEnumerable<string> urls)
        {
            IList<UrlResponse> responseTimeList = new List<UrlResponse>();

            foreach (var url in urls)
            {
                try
                {
                    var time = await CalculateTimeAsync(url);

                    var response = new UrlResponse
                    {
                        Url = url,
                        ResponseTimeMs = time
                    };

                    responseTimeList.Add(response);
                }
                catch (Exception e)
                {
                    _consoleWrapper.Write(e.Message);
                }
            }

            return responseTimeList;
        }

        private async Task<int> CalculateTimeAsync(string url)
        {
            var timer = Stopwatch.StartNew();

            using var response = await _httpClientService.GetAsync(url);

            timer.Stop();

            return (int)timer.ElapsedMilliseconds;
        }
    }
}