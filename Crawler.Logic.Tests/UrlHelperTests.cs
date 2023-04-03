
using Crawler.Logic.Helpers;

namespace Crawler.Logic.Tests
{
    internal class UrlHelperTests
    {
        private UrlHelper _helper;

        [SetUp]
        public void Setup()
        {
            _helper = new UrlHelper();
        }

        [Test]
        public void GetAbsoluteUrl_PathAndBaseUrl_ReturnAbsoluteUrl()
        {
            var path = "/docs/";
            var baseUrl = "https://www.litedb.org";

            var absoluteUrl = _helper.GetAbsoluteUrl(baseUrl, path);

            Assert.That(absoluteUrl == "https://www.litedb.org/docs");
        }
    }
}
