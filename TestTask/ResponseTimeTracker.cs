using System.Diagnostics;
using CrawlerLogic.Models;

namespace CrawlerLogic
{
    public class ResponseTimeTracker
    {
        private readonly HttpClient _httpClient;

        public ResponseTimeTracker(HttpClient client)
        {
            _httpClient = client;
        }

        public async Task<IList<ResponseTime>> GetResponseTimeAsync(IEnumerable<string> urls) //method gets response time of each url and sorts it ascending
        {
            IList<ResponseTime> responseTimeList = new List<ResponseTime>();

            foreach (var url in urls)
            {
                try
                {
                    var time = await CalculateTimeAsync(url);

                    var response = new ResponseTime(url, time);

                    responseTimeList.Add(response);
                }
                catch (Exception)
                {
                    //Console.WriteLine($"\nCant access url: {url}\n" + e.Message);
                }
            }

            return responseTimeList.OrderBy(x => x._responseTime).ToList();
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