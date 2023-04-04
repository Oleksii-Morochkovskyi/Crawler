using Crawler.Logic.Crawlers;
using Crawler.Logic.Helpers;
using Crawler.Logic.Interfaces;
using Crawler.Logic.Services;
using Crawler.Logic.Validators;
using Moq;
using NUnit.Framework;

namespace Crawler.Logic.Tests.Crawlers.Test
{
    internal class XmlCrawlerTests
    {
        private UrlHelper _urlHelper;
        private UrlValidator _validator;
        private Mock<IConsoleHandler> _consoleHandlerMock;
        private Mock<HttpClientService> _httpClientServiceMock;
        private HttpClient _httpClient;
        private XmlCrawler _crawler;
        
        [SetUp]
        public void SetUp()
        {
            _urlHelper = new UrlHelper();
            _validator = new UrlValidator();
            _consoleHandlerMock = new Mock<IConsoleHandler>();
            _httpClient = new HttpClient();
            _httpClientServiceMock = new Mock<HttpClientService>(_httpClient);
            _crawler = new XmlCrawler(_consoleHandlerMock.Object, _urlHelper, _validator,
                _httpClientServiceMock.Object);

        }
        
        [Test]
        public async Task CrawlAsync_ValidUrl_ReturnsListWithUrls()
        {
            var url = "https://www.example.com";
            var xmlString = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><urlset xmlns=\"1\"><url><loc>https://example.com/page1</loc></url><url><loc>https://example.com/page2</loc></url><url><loc>https://example.com/page3</loc></url></urlset>";

            ICollection<string> expectedUrls = new HashSet<string>
            {
                "https://example.com/page1",
                "https://example.com/page2",
                "https://example.com/page3"
            };

            _httpClientServiceMock.Setup(x => x.GetStringAsync(url+ "/sitemap.xml")).ReturnsAsync(xmlString);

            var result = await _crawler.CrawlAsync(url);

            Assert.That(expectedUrls,Is.EqualTo(result));
        }
    }
}
