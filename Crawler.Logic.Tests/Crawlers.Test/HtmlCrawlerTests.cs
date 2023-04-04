using Crawler.Logic.Crawlers;
using Crawler.Logic.Helpers;
using Crawler.Logic.Interfaces;
using Crawler.Logic.Parsers;
using Crawler.Logic.Services;
using Crawler.Logic.Validators;
using Moq;
using NUnit.Framework;

namespace Crawler.Logic.Tests.Crawlers.Test
{
    internal class HtmlCrawlerTests
    {
        private Mock<IConsoleHandler> _consoleMock;
        private Mock<HttpClientService> _httpClientMock;
        private UrlValidator _urlValidator;
        private HttpClient _httpClient;
        private HtmlCrawler _crawler;
        private HtmlParser _parser;
        private UrlHelper _urlHelper;

        [SetUp]
        public void SetUp()
        {
            _httpClient = new HttpClient();
            _urlValidator = new UrlValidator();
            _urlHelper = new UrlHelper();
            _consoleMock = new Mock<IConsoleHandler>();
            _httpClientMock = new Mock<HttpClientService>(_httpClient);
            _parser = new HtmlParser(_httpClientMock.Object, _urlHelper);
            _crawler = new HtmlCrawler(_consoleMock.Object, _parser, _urlValidator);
        }

        [Test]
        public async Task CrawlAsync_BaseUrl_ReturnsListWithCorrectUrls()
        {
            var url = "https://example.com";
            var html = "<html><body><a href=\"/page2\">Link 1</a><a href=\"/page3\">Link 2</a></body></html>";

            ICollection<string> expectedResult = new HashSet<string>
            {
                "https://example.com",
                "https://example.com/page2",
                "https://example.com/page3"
            };
            _httpClientMock.Setup(c => c.GetStringAsync(url)).ReturnsAsync(html);

            var result = await _crawler.CrawlAsync(url);

            Assert.That(expectedResult, Is.EqualTo(result));
        }
    }
}
