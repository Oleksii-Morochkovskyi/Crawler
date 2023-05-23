using Crawler.Application.Crawlers;
using Crawler.Application.Helpers;
using Crawler.Application.Interfaces;
using Crawler.Application.Services;
using Crawler.Application.Validators;
using Moq;
using NUnit.Framework;

namespace Crawler.Logic.Tests.Crawlers
{
    public class XmlCrawlerTests
    {
        private Mock<UrlHelper> _urlHelperMock;
        private Mock<UrlValidator> _validatorMock;
        private Mock<IConsoleHandler> _consoleHandlerMock;
        private Mock<HttpClientService> _httpClientServiceMock;
        private HttpClient _httpClient;
        private XmlCrawler _crawler;

        [SetUp]
        public void SetUp()
        {
            _urlHelperMock = new Mock<UrlHelper>();
            _validatorMock = new Mock<UrlValidator>();
            _consoleHandlerMock = new Mock<IConsoleHandler>();
            _httpClient = new HttpClient();
            _httpClientServiceMock = new Mock<HttpClientService>(_httpClient);
            _crawler = new XmlCrawler(_consoleHandlerMock.Object, _urlHelperMock.Object, _validatorMock.Object, _httpClientServiceMock.Object);

        }

        [Test]
        public async Task CrawlAsync_ValidUrl_ReturnsListWithUrls()
        {
            var url = "https://www.example.com";
            var xmlString = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><urlset xmlns=\"1\"><url><loc>/page1/</loc></url><url><loc>https://www.example.com/page1.svg</loc></url><url><loc>https://windows.com</loc></url></urlset>";

            var firstUrl = "/page1/";
            var secondUrl = "https://www.example.com/page1.svg";
            var thirdUrl = "https://windows.com";
            var absoluteUrl = "https://www.example.com/page1";

            ICollection<string> expectedUrls = new HashSet<string>
            {
                "https://www.example.com/page1",
            };

            _httpClientServiceMock.Setup(x => x.GetStringAsync(url + "/sitemap.xml")).ReturnsAsync(xmlString);

            _urlHelperMock.Setup(x => x.GetAbsoluteUrl(url, firstUrl)).Returns(absoluteUrl);
            _urlHelperMock.Setup(x => x.GetAbsoluteUrl(url, secondUrl)).Returns(secondUrl);
            _urlHelperMock.Setup(x => x.GetAbsoluteUrl(url, thirdUrl)).Returns(thirdUrl);

            _validatorMock.Setup(x => x.IsHtmlDoc(absoluteUrl, url)).Returns(true);
            _validatorMock.Setup(x => x.IsHtmlDoc(secondUrl, url)).Returns(false);
            _validatorMock.Setup(x => x.IsHtmlDoc(thirdUrl, url)).Returns(false);

            var result = await _crawler.CrawlAsync(url);

            Assert.That(expectedUrls, Is.EqualTo(result));
        }

        [Test]
        public async Task CrawlAsync_UrlOfSiteWithoutSitemap_ShouldThrowException()
        {
            var url = "https://www.example.com";

            _httpClientServiceMock.Setup(x => x.GetStringAsync(url + "/sitemap.xml")).ThrowsAsync(new Exception("Test exception"));

            var result = await _crawler.CrawlAsync(url);

            _consoleHandlerMock.Verify(x => x.Write("Test exception"));
        }
    }
}
