using Crawler.Logic.Helpers;
using Crawler.Logic.Parsers;
using Moq;
using Crawler.Logic.Interfaces;
using NUnit.Framework;
using Crawler.Logic.Services;

namespace Crawler.Logic.Tests
{
    internal class HtmlParserTests
    {
        private HtmlParser _parser;
        private Mock<HttpClientService> _httpClientMock;
        private UrlHelper _urlHelper;
        private HttpClient _httpClient;

        [SetUp]
        public void SetUp()
        {
            _httpClient = new HttpClient();
            _urlHelper = new UrlHelper();
            _httpClientMock = new Mock<HttpClientService>(_httpClient);
            _parser = new HtmlParser(_httpClientMock.Object, _urlHelper);
        }

        [Test]
        public async Task ParseAsync_Should_Return_Correct_Urls()
        {
            // Arrange
            var baseUrl = "https://example.com";
            var url = "https://example.com/page1";

            var html = "<html><body><a href=\"/page2\">Link 1</a><a href=\"/page3\">Link 2</a></body></html>";
            _httpClientMock.Setup(c => c.GetStringAsync(url)).ReturnsAsync(html);

            // Act
            var result = await _parser.ParseAsync(baseUrl, url);

            // Assert
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That( result.Contains("https://example.com/page2"));
            Assert.That(result.Contains("https://example.com/page3"));
        }
    }
}
