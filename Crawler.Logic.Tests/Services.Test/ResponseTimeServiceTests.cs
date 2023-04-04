using System.Net;
using Crawler.Logic.Interfaces;
using Crawler.Logic.Models;
using Crawler.Logic.Services;
using Moq;
using NUnit.Framework;

namespace Crawler.Logic.Tests.Services.Test
{
    internal class ResponseTimeServiceTests
    {
        private ResponseTimeService _responseTimeService;
        private HttpClient _client;
        private Mock<HttpClientService> _httpClientServiceMock;
        private Mock<IConsoleHandler> _consoleHandlerMock;

        [SetUp]
        public void SetUp()
        {
            _consoleHandlerMock = new Mock<IConsoleHandler>();
            _client = new HttpClient();
            _httpClientServiceMock = new Mock<HttpClientService>(_client);
            _responseTimeService = new ResponseTimeService(_httpClientServiceMock.Object, _consoleHandlerMock.Object);
        }

        [Test]
        public async Task GetResponseTimeAsync_ListWithUrls_ReturnsListOfUrlResponseInstancesWithCalculatedResponseTimeForEachUrl()
        {
            var urls = new List<string>
            {
                "https://www.litedb.org",
                "https://openai.com",
                "https://www.google.com/"
            };

            _httpClientServiceMock.Setup(x => x.GetAsync(It.IsAny<string>()))
                .Callback(() =>  Thread.Sleep(1000))
                .ReturnsAsync(new HttpResponseMessage());
           
            var result = await _responseTimeService.GetResponseTimeAsync(urls);

            Assert.That(result.All(x => x.ResponseTimeMs >= 1000));
        }
    }
}
