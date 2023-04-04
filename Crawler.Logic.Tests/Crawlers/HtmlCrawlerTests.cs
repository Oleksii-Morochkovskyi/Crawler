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
        private Mock<UrlValidator> _urlValidator;
        private HttpClient _httpClient;
        private HtmlCrawler _crawler;
        private Mock<HtmlParser> _parserMock;
        private Mock<UrlHelper> _urlHelper;

        [SetUp]
        public void SetUp()
        {
            _httpClient = new HttpClient();
            _urlValidator = new Mock<UrlValidator>();
            _urlHelper = new Mock<UrlHelper>();
            _consoleMock = new Mock<IConsoleHandler>();
            _httpClientMock = new Mock<HttpClientService>(_httpClient);
            _parserMock = new Mock<HtmlParser>(_httpClientMock.Object, _urlHelper.Object);
            _crawler = new HtmlCrawler(_consoleMock.Object, _parserMock.Object, _urlValidator.Object);
        }

        [Test]
        public async Task CrawlAsync_BaseUrl_ReturnsListWithCorrectUrls()
        {
            var url = "https://example.com";

            ICollection<string> parsedUrls = new HashSet<string>
            {
                "https://example.com/page2",
                "https://example.com/123.svg"
            };

            ICollection<string> expectedResult = new HashSet<string>
            {
                "https://example.com",
                "https://example.com/page2"
            };

            _parserMock.Setup(x => x.ParseAsync(url, url)).ReturnsAsync(parsedUrls);
            _parserMock.Setup(x => x.ParseAsync(url, parsedUrls.First())).ThrowsAsync(new Exception("Test exception"));
            
            _urlValidator.SetupSequence(x=>x.IsCorrectFormat(parsedUrls.First(),url)).Returns(true)
                .Returns(false);
            _urlValidator.SetupSequence(x => x.IsHtmlDoc(parsedUrls.First(), url)).Returns(true)
                .Returns(false);
            
            var result = await _crawler.CrawlAsync(url);

            Assert.That(expectedResult, Is.EqualTo(result));
        }

        [Test]
        public async Task CrawlAsync_ThrowsException_CatchesExceptionPrintsExceptionMessage()
        {
            var url = "https://example.com";

            _parserMock.SetupSequence(x => x.ParseAsync(url, url)).ThrowsAsync(new Exception("Test exception"));

            var result = await _crawler.CrawlAsync(url);

            _consoleMock.Verify(x => x.Write("Test exception"));
        }
    }
}
