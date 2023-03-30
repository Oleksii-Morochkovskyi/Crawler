using System.Diagnostics;
using Crawler.Logic.Interfaces;
using Crawler.Logic.Models;

namespace Crawler.Logic.Services
{
    public class ResponseTimeTracker
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger _logger;

        public ResponseTimeTracker(HttpClient client, ILogger logger)
        {
            _httpClient = client;
            _logger = logger;
        }

        public async Task<IList<UrlResponse>> GetResponseTimeAsync(IEnumerable<string> urls)
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
                        ResponseTime = time
                    };

                    responseTimeList.Add(response);
                }
                catch (Exception e)
                {
                    _logger.Write(e.Message);
                }
            }

            return responseTimeList.OrderBy(x => x.ResponseTime)
                .ToList();
        }

        private async Task<int> CalculateTimeAsync(string url)
        {
            var timer = Stopwatch.StartNew();

            using var response = await _httpClient.GetAsync(url);

            timer.Stop();

            return (int)timer.ElapsedMilliseconds;
        }
    }
}