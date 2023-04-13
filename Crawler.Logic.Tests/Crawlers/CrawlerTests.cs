using Crawler.WebApp;
using Crawler.Logic.Crawlers;
using Crawler.Logic.Enums;
using Crawler.Logic.Interfaces;
using Crawler.Logic.Models;
using Crawler.Logic.Parsers;
using Crawler.Logic.Services;
using Crawler.Logic.Validators;
using Moq;
using NUnit.Framework;
using UrlHelper = Crawler.Logic.Helpers.UrlHelper;

namespace Crawler.Logic.Tests.Crawlers
{
    public class CrawlerTests
    {
        private UrlValidator _validator;
        private UrlHelper _helper;
        private HttpClient _httpClient;
        private HttpClientService _httpClientService;
        private IConsoleHandler _consoleHandler;
        private HtmlParser _htmlParser;
        private Mock<HtmlCrawler> _htmlCrawlerMock;
        private Mock<XmlCrawler> _xmlCrawlerMock;
        private Mock<ResponseTimeService> _responseTimeServiceMock;
        private Logic.Crawlers.Crawler _crawler;

        [SetUp]
        public void SetUp()
        {
            _consoleHandler = new ConsoleWrapper();
            _validator = new UrlValidator();
            _httpClient = new HttpClient();
            _httpClientService = new HttpClientService(_httpClient);
            _helper = new UrlHelper();
            _htmlParser = new HtmlParser(_httpClientService, _helper);
            _htmlCrawlerMock = new Mock<HtmlCrawler>(_consoleHandler, _htmlParser, _validator);
            _xmlCrawlerMock = new Mock<XmlCrawler>(_consoleHandler, _helper, _validator, _httpClientService);
            _responseTimeServiceMock = new Mock<ResponseTimeService>(_httpClientService, _consoleHandler);
            _crawler = new Logic.Crawlers.Crawler(_responseTimeServiceMock.Object, _htmlCrawlerMock.Object, _xmlCrawlerMock.Object);
        }

        [Test]
        public async Task StartCrawlerAsync_Url_SetsUrlLocation()
        {
            var url = "https://example.com";

            ICollection<string> htmlCrawlResults = new List<string> { "https://apple.tv", "https://windows.com" };
            ICollection<string> xmlCrawlResults = new List<string> { "https://samsung.com", "https://windows.com" };
            IEnumerable<UrlResponse> urlResponseServiceResults = new List<UrlResponse>
            {
                new UrlResponse {Url = "https://apple.tv", ResponseTimeMs = 10},
                new UrlResponse {Url = "https://windows.com", ResponseTimeMs = 15},
                new UrlResponse {Url = "https://samsung.com", ResponseTimeMs = 20}
            };

            var allUrls = htmlCrawlResults.Union(xmlCrawlResults);

            _htmlCrawlerMock.Setup(x => x.CrawlAsync(url)).ReturnsAsync(htmlCrawlResults);
            _xmlCrawlerMock.Setup(x => x.CrawlAsync(url)).ReturnsAsync(xmlCrawlResults);
            _responseTimeServiceMock.Setup(x => x.GetResponseTimeAsync(allUrls))
                .ReturnsAsync(urlResponseServiceResults);

            var result = await _crawler.StartCrawlerAsync(url);

            Assert.That(result.Count(), Is.EqualTo(3));
            Assert.That(result.Count(x => x.Location == Location.Html), Is.EqualTo(1));
            Assert.That(result.Count(x => x.Location == Location.Xml), Is.EqualTo(1));
            Assert.That(result.Count(x => x.Location == Location.Both), Is.EqualTo(1));
        }

        [Test]
        public async Task StartCrawlerAsync_Url_SortsUrlsByTimeResponse()
        {
            var url = "https://example.com";

            ICollection<string> htmlCrawlResults = new List<string> { "https://apple.tv", "https://windows.com" };
            ICollection<string> xmlCrawlResults = new List<string> { "https://samsung.com", "https://windows.com" };
            IEnumerable<UrlResponse> urlResponseServiceResults = new List<UrlResponse>
            {
                new UrlResponse {Url = "https://apple.tv", ResponseTimeMs = 20},
                new UrlResponse {Url = "https://windows.com", ResponseTimeMs = 10},
                new UrlResponse {Url = "https://samsung.com", ResponseTimeMs = 15}
            };

            var allUrls = htmlCrawlResults.Union(xmlCrawlResults);

            _htmlCrawlerMock.Setup(x => x.CrawlAsync(url)).ReturnsAsync(htmlCrawlResults);
            _xmlCrawlerMock.Setup(x => x.CrawlAsync(url)).ReturnsAsync(xmlCrawlResults);
            _responseTimeServiceMock.Setup(x => x.GetResponseTimeAsync(allUrls)).ReturnsAsync(urlResponseServiceResults);

            var result = await _crawler.StartCrawlerAsync(url);

            Assert.True(result.First().ResponseTimeMs == 10);
            Assert.True(result.Last().ResponseTimeMs == 20);
        }

        [Test]
        public async Task StartCrawlerAsync_Url_GetsResponseTime()
        {
            var url = "https://example.com";

            ICollection<string> htmlCrawlResults = new List<string> { "https://apple.tv", "https://windows.com" };
            ICollection<string> xmlCrawlResults = new List<string> { "https://samsung.com", "https://windows.com" };
            IEnumerable<UrlResponse> urlResponseServiceResults = new List<UrlResponse>
            {
                new UrlResponse {Url = "https://apple.tv", ResponseTimeMs = 20},
                new UrlResponse {Url = "https://windows.com", ResponseTimeMs = 10},
                new UrlResponse {Url = "https://samsung.com", ResponseTimeMs = 15}
            };

            var allUrls = htmlCrawlResults.Union(xmlCrawlResults);

            _htmlCrawlerMock.Setup(x => x.CrawlAsync(url)).ReturnsAsync(htmlCrawlResults);
            _xmlCrawlerMock.Setup(x => x.CrawlAsync(url)).ReturnsAsync(xmlCrawlResults);
            _responseTimeServiceMock.Setup(x => x.GetResponseTimeAsync(allUrls)).ReturnsAsync(urlResponseServiceResults);

            var result = await _crawler.StartCrawlerAsync(url);

            Assert.True(result.All(x => x.ResponseTimeMs != null));
        }
    }
}
