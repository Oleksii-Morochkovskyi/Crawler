using Crawler.Logic.Crawlers;
using Crawler.Logic.Enums;
using Crawler.Logic.Helpers;
using Crawler.Logic.Interfaces;
using Crawler.Logic.Models;
using Crawler.Logic.Parsers;
using Crawler.Logic.Services;
using Crawler.Logic.Validators;
using Crawler.Persistence.Interfaces;
using Moq;
using NUnit.Framework;

namespace Crawler.ConsoleOutput.Tests
{
    public class ConsoleProcessorTests
    {
        private ConsoleProcessor _console;
        private Mock<IConsoleHandler> _writerMock;
        private Mock<Logic.Crawlers.Crawler> _crawlerMock;
        private Mock<UrlValidator> _validatorMock;
        private ResponseTimeService _responseTimeService;
        private HtmlCrawler _htmlCrawler;
        private XmlCrawler _xmlCrawler;
        private Mock<UrlHelper> _helperMock;
        private HtmlParser _parser;
        private HttpClientService _httpClientService;
        private HttpClient _httpClient;
        private Mock<DatabaseInteraction> _dbInteractionMock;
        private Mock<IFoundUrlRepository> _foundUrlRepositoryMock;
        private Mock<IInitialUrlRepository> _initialUrlRepositoryMock;

        [SetUp]
        public void Setup()
        {
            _foundUrlRepositoryMock = new Mock<IFoundUrlRepository>();
            _initialUrlRepositoryMock = new Mock<IInitialUrlRepository>();
            _dbInteractionMock = new Mock<DatabaseInteraction>(_foundUrlRepositoryMock.Object, _initialUrlRepositoryMock.Object);
            _httpClient = new HttpClient();
            _httpClientService = new HttpClientService(_httpClient);
            _validatorMock = new Mock<UrlValidator>();
            _writerMock = new Mock<IConsoleHandler>();
            _helperMock = new Mock<UrlHelper>();
            _responseTimeService = new ResponseTimeService(_httpClientService, _writerMock.Object);
            _xmlCrawler = new XmlCrawler(_writerMock.Object, _helperMock.Object, _validatorMock.Object, _httpClientService);
            _parser = new HtmlParser(_httpClientService, _helperMock.Object);
            _htmlCrawler = new HtmlCrawler(_writerMock.Object, _parser, _validatorMock.Object);
            _crawlerMock = new Mock<Logic.Crawlers.Crawler>(_responseTimeService, _htmlCrawler, _xmlCrawler);
            _console = new ConsoleProcessor(_writerMock.Object, _validatorMock.Object, _crawlerMock.Object, _dbInteractionMock.Object);
            
        }

        [Test]
        public void GetAddress_ValidUrl_ReturnUrl()
        {
            var url = "https://www.litedb.org";
            _writerMock.Setup(x => x.Read()).Returns(url);
            _validatorMock.Setup(x => x.IsValidUrl(url)).Returns(true);

            var input = _console.GetAddress();

            Assert.That(input, Is.EqualTo("https://www.litedb.org"));
        }

        [Test]
        public void GetAddress_InvalidUrlThenValidUrl_PrintsErrorMessageAndStaysInLoop()
        {
            var url = "https://www.litedb.org";

            _writerMock.SetupSequence(x => x.Read())
                .Returns("123")
                .Returns(url);
            _validatorMock.SetupSequence(x => x.IsValidUrl("123")).Returns(false);
            _validatorMock.SetupSequence(x => x.IsValidUrl(url)).Returns(true);

            var input = _console.GetAddress();

            _writerMock.Verify(x => x.Write("Enter URL: "), Times.Exactly(2));
            _writerMock.Verify(x => x.Write("You have entered wrong url. Please try again...\n"));
            Assert.That(input, Is.EqualTo(url));
        }

        [Test]
        public async Task PrintResult_UrlsWithSetLocation_PrintsUrlFoundInHtml()
        {
            var url = "https://example.com";

            IEnumerable<UrlResponse> results = new List<UrlResponse>
            {
                new UrlResponse{Url = "https://home.com", Location = Location.Html},
                new UrlResponse{Url = "https://windows.com", Location = Location.Xml},
                new UrlResponse{Url = "https://apple.com", Location = Location.Both}
            };

            _writerMock.Setup(x => x.Read()).Returns(url);
            _validatorMock.SetupSequence(x => x.IsValidUrl(url)).Returns(true);
            _crawlerMock.Setup(x => x.StartCrawlerAsync(url)).ReturnsAsync(results);

            await _console.ExecuteAsync();

            _writerMock.Verify(x => x.Write("\nUrls FOUND BY CRAWLING THE WEBSITE but not in sitemap.xml: \n"));
            _writerMock.Verify(x => x.Write("https://home.com"));
        }

        [Test]
        public async Task PrintResult_UrlsWithSetLocation_PrintsUrlFoundInXml()
        {
            var url = "https://example.com";

            IEnumerable<UrlResponse> results = new List<UrlResponse>
            {
                new UrlResponse{Url = "https://home.com", Location = Location.Html},
                new UrlResponse{Url = "https://windows.com", Location = Location.Xml},
                new UrlResponse{Url = "https://apple.com", Location = Location.Both}
            };

            _writerMock.Setup(x => x.Read()).Returns(url);
            _validatorMock.SetupSequence(x => x.IsValidUrl(url)).Returns(true);
            _crawlerMock.Setup(x => x.StartCrawlerAsync(url)).ReturnsAsync(results);

            await _console.ExecuteAsync();

            _writerMock.Verify(x => x.Write("\nUrls FOUND IN SITEMAP.XML but not founded after crawling a website: \n"));
            _writerMock.Verify(x => x.Write("https://windows.com"));
        }

        [Test]
        public async Task PrintResult_UrlsWithResponseTime_PrintsUrlsWithTimeResponse()
        {
            var url = "https://example.com";

            var results = new List<UrlResponse>
            {
                new UrlResponse{Url = "https://samsung.com", ResponseTimeMs = 10, Location = Location.Xml},
                new UrlResponse{Url = "https://windows.com", ResponseTimeMs = 15, Location = Location.Html},
                new UrlResponse{Url = "https://apple.com", ResponseTimeMs = 50, Location = Location.Both}
            };

            _writerMock.Setup(x => x.Read()).Returns(url);
            _validatorMock.SetupSequence(x => x.IsValidUrl(url)).Returns(true);
            _crawlerMock.Setup(x => x.StartCrawlerAsync(url)).ReturnsAsync(results);

            await _console.ExecuteAsync();

            _writerMock.Verify(x => x.Write("\n\nList with url and response time for each page: \n"));
            _writerMock.Verify(x => x.Write("URL".PadRight(70) + "Timing (ms)\n"));

            _writerMock.Verify(x => x.Write("https://samsung.com".PadRight(70) + "10ms"));
            _writerMock.Verify(x => x.Write("https://windows.com".PadRight(70) + "15ms"));
            _writerMock.Verify(x => x.Write("https://apple.com".PadRight(70) + "50ms"));
        }

        [Test]
        public async Task PrintResult_UrlsWithResponseTimeAndLocation_PrintsCountOfUrlsFromHtmlAndFromXml()
        {
            var url = "https://example.com";

            var results = new List<UrlResponse>
            {
                new UrlResponse{Url = "https://samsung.com", ResponseTimeMs = 10, Location = Location.Xml},
                new UrlResponse{Url = "https://windows.com", ResponseTimeMs = 15, Location = Location.Html},
                new UrlResponse{Url = "https://apple.com", ResponseTimeMs = 10, Location = Location.Xml},
            };

            _writerMock.Setup(x => x.Read()).Returns(url);
            _validatorMock.SetupSequence(x => x.IsValidUrl(url)).Returns(true);
            _crawlerMock.Setup(x => x.StartCrawlerAsync(url)).ReturnsAsync(results);

            await _console.ExecuteAsync();

            _writerMock.Verify(x => x.Write("\nUrls(html documents) found after crawling a website: 1"));
            _writerMock.Verify(x => x.Write("\nUrls found in sitemap: 2"));
        }
    }
}