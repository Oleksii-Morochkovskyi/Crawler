using System.Diagnostics;
using Crawler.Application.Interfaces;
using Crawler.Application.Services;
using Crawler.Application.Services.Interfaces;
using Crawler.Domain.Entities;

namespace Crawler.Logic.Services
{
    public class ResponseTimeService : IResponseTimeService
    {
        private readonly HttpClientService _httpClientService;
        private readonly IConsoleHandler _consoleHandler;

        public ResponseTimeService(
            HttpClientService clientService,
            IConsoleHandler consoleHandler)
        {
            _httpClientService = clientService;
            _consoleHandler = consoleHandler;
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
                    _consoleHandler.Write(e.Message);
                }
            }

            return responseTimeList;
        }

        public async Task<int> CalculateTimeAsync(string url)
        {
            var timer = Stopwatch.StartNew();

            using var response = await _httpClientService.GetAsync(url);

            timer.Stop();

            return (int)timer.ElapsedMilliseconds;
        }
    }
}