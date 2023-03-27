using System.Diagnostics;

namespace CrawlerLogic
{
    public class ResponseTimeTracker
    {
        private readonly HttpClient _httpClient;
        private readonly ResponseTimeModel _responseModel;

        public ResponseTimeTracker(HttpClient client)
        {
            _httpClient = client;
            _responseModel = new ResponseTimeModel();
        }

        public async Task<ResponseTimeModel> GetResponseTime(IEnumerable<string> urlList) //method gets response time of each url and sorts it ascending
        {
            foreach (var url in urlList)
            {
                try
                {
                    var time = await CalculateTime(url);

                    _responseModel.ResponseTime.Add(url, time);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"\nCant access url: {url}\n" + e.Message);
                }
            }

            _responseModel.ResponseTime = _responseModel.ResponseTime.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value); //sorting of response time ascending
            
            return _responseModel;
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