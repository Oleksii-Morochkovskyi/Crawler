using Crawler.Logic.Helpers;
using Crawler.Logic.Parsers;
using Moq;
using NUnit.Framework;
using Crawler.Logic.Services;

namespace Crawler.Logic.Tests.Parsers
{
    internal class HtmlParserTests
    {
        private HtmlParser _parser;
        private Mock<HttpClientService> _httpClientMock;
        private Mock<UrlHelper> _urlHelperMock;
        private HttpClient _httpClient;

        [SetUp]
        public void SetUp()
        {
            _httpClient = new HttpClient();
            _urlHelperMock = new Mock<UrlHelper>();
            _httpClientMock = new Mock<HttpClientService>(_httpClient);
            _parser = new HtmlParser(_httpClientMock.Object, _urlHelperMock.Object);
        }

        [Test]
        public async Task ParseAsync_BaseUrl_ShouldReturnCorrectUrls()
        {
            // Arrange
            var baseUrl = "https://example.com";
            var url = "https://example.com/page1";

            var html = "<html><body><a href=\"/page2\">Link 1</a><a href=\"/page3\">Link 2</a></body></html>";
            _httpClientMock.Setup(c => c.GetStringAsync(url)).ReturnsAsync(html);
            _urlHelperMock.Setup(x=>x.GetAbsoluteUrl(baseUrl, "/page2")).Returns("https://example.com/page2");
            _urlHelperMock.Setup(x => x.GetAbsoluteUrl(baseUrl, "/page3")).Returns("https://example.com/page3");

            // Act
            var result = await _parser.ParseAsync(baseUrl, url);

            // Assert
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result.Contains("https://example.com/page2"));
            Assert.That(result.Contains("https://example.com/page3"));
        }
    }
}
