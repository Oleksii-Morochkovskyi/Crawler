using System.Diagnostics;
using System.Net;


namespace CrawlerLogic
{
    public class ResponseTimeTracker
    {
        private readonly HttpClient _httpClient;

        public ResponseTimeTracker(HttpClient client)
        {
            _httpClient = client;
        }

        public async Task<Dictionary<string, int>> GetResponseTime(IEnumerable<string> urlList) //method gets response time of each url and sorts it ascending
        {
            var urlAndTimeResponse = new Dictionary<string, int>();

            foreach (var url in urlList)
            {
                try
                {
                    var time = await CalculateTime(url);

                    urlAndTimeResponse.Add(url, time);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"\nCant access url: {url}\n" + e.Message);
                }
            }

            return urlAndTimeResponse.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value); //sorting of response time ascending
        }

        private async Task<int> CalculateTime(string url)
        {
            var timer = Stopwatch.StartNew();

            using var response = await _httpClient.GetAsync(url);

            timer.Stop();

            return (int)timer.ElapsedMilliseconds;
        }
    }
}