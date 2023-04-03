using Azure;
using Crawler.Logic.Enums;
using Crawler.Logic.Interfaces;
using Crawler.Logic.Models;
using Microsoft.Kiota.Abstractions;
using Moq;
using static Microsoft.Graph.Constants;

namespace ConsoleOutput.Tests
{
    public class ConsoleProcessorTests
    {
        private ConsoleProcessor _console;
        private Mock<IOutputWriter> _writerMock;

        [SetUp]
        public void Setup()
        {
            _writerMock = new Mock<IOutputWriter>();
            _console = new ConsoleProcessor(_writerMock.Object);
        }

        [Test]
        public void GetAddress_ReturnString()
        {
            _writerMock.Setup(x => x.Read()).Returns("https://www.litedb.org/");

            var url = _console.GetAddress();

            Assert.That(url, Is.EqualTo("https://www.litedb.org"));
        }

        [Test]
        public void PrintResult_ListOfUrlResponsesWithDifferentUrlsFromHtmlAndXml_PrintsInConsoleTwoListsWithDifferences()
        {
            var results = new List<UrlResponse>
            {
                new UrlResponse{Url = "url1", Location = Location.Html},
                new UrlResponse{Url = "url2", Location = Location.Html},
                new UrlResponse{Url = "url3", Location = Location.Xml},
                new UrlResponse{Url = "url4", Location = Location.Xml},
                new UrlResponse{Url = "url5", Location = Location.Both}
            };

            _console.PrintResult(results);

            _writerMock.Verify(x => x.Write("\nUrls FOUND BY CRAWLING THE WEBSITE but not in sitemap.xml: \n"));
            _writerMock.Verify(x => x.Write("url1"));
            _writerMock.Verify(x => x.Write("url2"));

            _writerMock.Verify(x => x.Write("\nUrls FOUND IN SITEMAP.XML but not founded after crawling a website: \n"));
            _writerMock.Verify(x => x.Write("url3"));
            _writerMock.Verify(x => x.Write("url4"));
        }

        [Test]
        public void PrintResult_ListOfUrlResponsesWithSameUrlsInHtmlAndXml_PrintsTwoListsWithDifferences()
        {
            var results = new List<UrlResponse>
            {
                new UrlResponse{Url = "url1", ResponseTimeMs = 10, Location = Location.Both}
            };

            _console.PrintResult(results);

            _writerMock.Verify(x => x.Write("\nOne of the ways to search for links did not bring any result or two ways of crawling brought same results.\n"));
        }

        [Test]
        public void PrintResult_ListOfUrlsWithResponseTime_PrintsListWithUrlsAndTimeResponses()
        {
            var results = new List<UrlResponse>
            {
                new UrlResponse{Url = "url1", ResponseTimeMs = 10, Location = Location.Xml},
                new UrlResponse{Url = "url2", ResponseTimeMs = 15, Location = Location.Html},
                new UrlResponse{Url = "url3", ResponseTimeMs = 50, Location = Location.Both}
            };

            _console.PrintResult(results);

            _writerMock.Verify(x => x.Write("\n\nList with url and response time for each page: \n"));
            _writerMock.Verify(x => x.Write("URL".PadRight(70) + "Timing (ms)\n"));

            _writerMock.Verify(x => x.Write("url1".PadRight(70) + "10ms"));
            _writerMock.Verify(x => x.Write("url2".PadRight(70) + "15ms"));
            _writerMock.Verify(x => x.Write("url3".PadRight(70) + "50ms"));
        }

        [Test]
        public void PrintResult_ListOfUrlsWithResponseTimeAndLocation_PrintsCountOfUrlsFromHtmlAndFromXml()
        {
            var results = new List<UrlResponse>
            {
                new UrlResponse{Url = "url1", ResponseTimeMs = 10, Location = Location.Xml},
                new UrlResponse{Url = "url2", ResponseTimeMs = 15, Location = Location.Html},
                new UrlResponse{Url = "url3", ResponseTimeMs = 10, Location = Location.Xml},
                new UrlResponse{Url = "url4", ResponseTimeMs = 15, Location = Location.Html},
                new UrlResponse{Url = "url5", ResponseTimeMs = 50, Location = Location.Html},
            };

            _console.PrintResult(results);

            _writerMock.Verify(x => x.Write("\nUrls(html documents) found after crawling a website: 3\n"));
            _writerMock.Verify(x => x.Write("\nUrls found in sitemap: 2"));
        }
    }
}