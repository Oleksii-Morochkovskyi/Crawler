using System.Diagnostics;
using Crawler.Logic.Interfaces;
using Crawler.Logic.Models;

namespace Crawler.Logic.Services
{
    public class ResponseTimeTracker
    {
        private readonly IHttpClient _httpClient;
        private readonly IOutputWriter _logger;

        public ResponseTimeTracker(IHttpClient client, IOutputWriter logger)
        {
            _httpClient = client;
            _logger = logger;
        }

        public async Task<IEnumerable<UrlResponse>> GetResponseTimeAsync(IEnumerable<string> urls)
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
                    _logger.Write(e.Message);
                }
            }

            return responseTimeList.OrderBy(x => x.ResponseTimeMs);
        }

        private async Task<int> CalculateTimeAsync(string url)
        {
            var timer = Stopwatch.StartNew();

            using var response = await _httpClient.GetAsync(url); //httpResponseMessage

            timer.Stop();

            return (int)timer.ElapsedMilliseconds;
        }
    }
}