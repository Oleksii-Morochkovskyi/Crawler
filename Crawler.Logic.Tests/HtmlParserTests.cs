using Crawler.Logic.Helpers;
using Crawler.Logic.Parsers;
using Crawler.Logic.Validators;
using System.Net.Http;
using Moq;
using System;
using System.Linq;
using Crawler.Logic.Interfaces;

namespace Crawler.Logic.Tests
{
    internal class HtmlParserTests
    {
        private HtmlParser _parser;
        private Mock<IHttpClient> _httpClientMock;
        private UrlHelper _urlHelper;

        [SetUp]
        public void SetUp()
        {
            _urlHelper = new UrlHelper();
            _httpClientMock = new Mock<IHttpClient>();

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
            Assert.That(result.Count, Is.EqualTo(3));
            Assert.That( result.Contains("https://example.com/page2"));
            Assert.That(result.Contains("https://example.com/page3"));
        }

    }
}
