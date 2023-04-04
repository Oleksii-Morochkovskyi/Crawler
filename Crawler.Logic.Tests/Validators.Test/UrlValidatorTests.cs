using Crawler.Logic.Validators;
using NUnit.Framework;

namespace Crawler.Logic.Tests.Validators.Test
{
    public class UrlValidatorTests
    {
        private UrlValidator _validator;

        [SetUp]
        public void Setup()
        {
            _validator = new UrlValidator();
        }

        [Test]
        public void IsValidUrl_ValidUrl_ReturnsTrue()
        {
            var url = "https://www.google.com/";

            var result = _validator.IsValidUrl(url);

            Assert.True(result);
        }

        [Test]
        public void IsValidUrl_InvalidUrl_ReturnsFalse()
        {
            var url = "123";

            var result = _validator.IsValidUrl(url);

            Assert.False(result);
        }

        [Test]
        public void IsCorrectFormat_UrlWithProperHost_ReturnsTrue()
        {
            var url = "https://www.litedb.org/docs";
            var baseUrl = "https://www.litedb.org";

            var result = _validator.IsCorrectFormat(url, baseUrl);

            Assert.True(result);
        }

        [Test]
        public void IsCorrectFormat_UrlWithHostOfAnotherSite_ReturnsFalse()
        {
            var url = "https://www.google.com/";
            var baseUrl = "https://www.litedb.org";

            var result = _validator.IsCorrectFormat(url, baseUrl);

            Assert.False(result);
        }

        [Test]
        public void IsHtmlDoc_UrlWhichRefersToHtmlDoc_ReturnsTrue()
        {
            var url = "https://www.litedb.org/docs";
            var baseUrl = "https://www.litedb.org";

            var result = _validator.IsHtmlDoc(url, baseUrl);

            Assert.True(result);
        }

        [Test]
        public void IsHtmlDoc_UrlWhichRefersToSvgFile_ReturnsFalse()
        {
            var url = "https://upload.wikimedia.org/wikipedia/commons/0/02/SVG_logo.svg";
            var baseUrl = "https://upload.wikimedia.org";

            var result = _validator.IsHtmlDoc(url, baseUrl);

            Assert.False(result);
        }
    }
}