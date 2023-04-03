using System.Collections;
using Crawler.Logic.Crawlers;
using NUnit.Framework;
using Crawler.Logic.Enums;
using Crawler.Logic.Helpers;
using Crawler.Logic.Interfaces;
using Crawler.Logic.Models;
using Crawler.Logic.Parsers;
using Crawler.Logic.Services;
using Crawler.Logic.Validators;
using Moq;
using static Microsoft.Graph.Constants;

namespace Crawler.ConsoleOutput.Tests
{
    public class ConsoleProcessorTests
    {
        private ConsoleProcessor _console;
        private Mock<IOHandler> _writerMock;
        private Mock<Logic.Crawlers.Crawler> _crawlerMock;
        private UrlValidator _validator;
        private ResponseTimeService _responseTimeService;
        private HtmlCrawler _htmlCrawler;
        private XmlCrawler _xmlCrawler;
        private UrlHelper _helper;
        private HtmlParser _parser;
        private HttpClientService _httpClient;



        [SetUp]
        public void Setup()
        {
            _validator = new UrlValidator();
            _writerMock = new Mock<IOHandler>();
            _helper = new UrlHelper();
            _responseTimeService = new ResponseTimeService(_httpClient, _writerMock.Object);
            _xmlCrawler = new XmlCrawler(_writerMock.Object, _helper, _validator);
            _parser = new HtmlParser(_httpClient, _helper);
            _htmlCrawler = new HtmlCrawler(_writerMock.Object, _parser, _validator);
            _crawlerMock = new Mock<Logic.Crawlers.Crawler>(_responseTimeService, _htmlCrawler, _xmlCrawler);
            _console = new ConsoleProcessor(_writerMock.Object, _validator, _crawlerMock.Object);
        }

        [Test]
        public void GetAddress_ReturnString()
        {
            _writerMock.Setup(x => x.Read()).Returns("https://www.litedb.org/");

            var url = _console.GetAddress();

            Assert.That(url, Is.EqualTo("https://www.litedb.org"));
        }

        [Test]
        public async Task PrintResult_ListOfUrlResponsesWithDifferentUrlsFromHtmlAndXml_PrintsInConsoleTwoListsWithDifferences()
        {
            var url = "https://example.com";

            IEnumerable<UrlResponse> results = new List<UrlResponse>
            {
                new UrlResponse{Url = "url1", Location = Location.Html},
                new UrlResponse{Url = "url2", Location = Location.Html},
                new UrlResponse{Url = "url3", Location = Location.Xml},
                new UrlResponse{Url = "url4", Location = Location.Xml},
                new UrlResponse{Url = "url5", Location = Location.Both}
            };

            _writerMock.Setup(x => x.Read()).Returns(url);
            _crawlerMock.Setup(x => x.StartCrawlerAsync(url)).ReturnsAsync(results);

            await _console.PrintResult();

            _writerMock.Verify(x => x.Write("\nUrls FOUND BY CRAWLING THE WEBSITE but not in sitemap.xml: \n"));
            _writerMock.Verify(x => x.Write("url1"));
            _writerMock.Verify(x => x.Write("url2"));

            _writerMock.Verify(x => x.Write("\nUrls FOUND IN SITEMAP.XML but not founded after crawling a website: \n"));
            _writerMock.Verify(x => x.Write("url3"));
            _writerMock.Verify(x => x.Write("url4"));
        }

        [Test]
        public async Task PrintResult_ListOfUrlResponsesWithSameUrlsInHtmlAndXml_PrintsTwoListsWithDifferences()
        {
            var url = "https://example.com";

            var results = new List<UrlResponse>
            {
                new UrlResponse{Url = "url1", ResponseTimeMs = 10, Location = Location.Both}
            };

            _writerMock.Setup(x => x.Read()).Returns(url);
            _crawlerMock.Setup(x => x.StartCrawlerAsync(url)).ReturnsAsync(results);

            await _console.PrintResult();

            _writerMock.Verify(x => x.Write("\nOne of the ways to search for links did not bring any result or two ways of crawling brought same results.\n"));
        }

        [Test]
        public async Task PrintResult_ListOfUrlsWithResponseTime_PrintsListWithUrlsAndTimeResponses()
        {
            var url = "https://example.com";

            var results = new List<UrlResponse>
            {
                new UrlResponse{Url = "url1", ResponseTimeMs = 10, Location = Location.Xml},
                new UrlResponse{Url = "url2", ResponseTimeMs = 15, Location = Location.Html},
                new UrlResponse{Url = "url3", ResponseTimeMs = 50, Location = Location.Both}
            };

            _writerMock.Setup(x => x.Read()).Returns(url);
            _crawlerMock.Setup(x => x.StartCrawlerAsync(url)).ReturnsAsync(results);

            await _console.PrintResult();

            _writerMock.Verify(x => x.Write("\n\nList with url and response time for each page: \n"));
            _writerMock.Verify(x => x.Write("URL".PadRight(70) + "Timing (ms)\n"));

            _writerMock.Verify(x => x.Write("url1".PadRight(70) + "10ms"));
            _writerMock.Verify(x => x.Write("url2".PadRight(70) + "15ms"));
            _writerMock.Verify(x => x.Write("url3".PadRight(70) + "50ms"));
        }

        [Test]
        public async Task PrintResult_ListOfUrlsWithResponseTimeAndLocation_PrintsCountOfUrlsFromHtmlAndFromXml()
        {
            var url = "https://example.com";

            var results = new List<UrlResponse>
            {
                new UrlResponse{Url = "url1", ResponseTimeMs = 10, Location = Location.Xml},
                new UrlResponse{Url = "url2", ResponseTimeMs = 15, Location = Location.Html},
                new UrlResponse{Url = "url3", ResponseTimeMs = 10, Location = Location.Xml},
                new UrlResponse{Url = "url4", ResponseTimeMs = 15, Location = Location.Html},
                new UrlResponse{Url = "url5", ResponseTimeMs = 50, Location = Location.Html},
            };

            _writerMock.Setup(x => x.Read()).Returns(url);
            _crawlerMock.Setup(x => x.StartCrawlerAsync(url)).ReturnsAsync(results);

            await _console.PrintResult();

            _writerMock.Verify(x => x.Write("\nUrls(html documents) found after crawling a website: 3"));
            _writerMock.Verify(x => x.Write("\nUrls found in sitemap: 2"));
        }
    }
}