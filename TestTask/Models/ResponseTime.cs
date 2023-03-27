
namespace CrawlerLogic.Models
{
    public class ResponseTime
    {
        public readonly string _url;
        public readonly int _responseTime;

        public ResponseTime(string url, int responseTime )
        {
            _url = url;
            _responseTime = responseTime;
        }
    }
}
