using System.Diagnostics;


namespace CrawlerLogic
{
    public class Timer
    {
        public async Task<Dictionary<string,long>> GetResponseTime(IEnumerable<string> urlList) //method gets response time of each url and sorts it ascending
        {
            var urlAndTimeResponse = new Dictionary<string, long>();

            using var httpClient = new HttpClient();

            foreach (var url in urlList)
            {
                try
                {
                    var time = await CalculateTime(url); 

                    urlAndTimeResponse.Add(url,time);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"\nCant access url: {url}\n" + e.Message);
                }
            }

            return urlAndTimeResponse.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value); //sorting of response time ascending
        }

        private async Task<long> CalculateTime(string url)
        {
            using var httpClient = new HttpClient();

            var timer = Stopwatch.StartNew();

            using var response = await httpClient.GetAsync(url);

            timer.Stop();

            return timer.ElapsedMilliseconds;
        }
    }
}
