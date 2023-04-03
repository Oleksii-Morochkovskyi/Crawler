using Crawler.Logic.Validators;

namespace Crawler.Logic.Tests
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

            Assert.That(result, Is.True);
        }

        [Test]
        public void IsValidUrl_InvalidUrl_ReturnsFalse()
        {
            var url = "123";

            var result = _validator.IsValidUrl(url);

            Assert.That(result, Is.False);
        }

        [Test]
        public void IsCorrectFormat_CorrectUrl_ReturnsTrue()
        {
            var url = "https://www.litedb.org/docs";
            var baseUrl = "https://www.litedb.org";

            var result = _validator.IsCorrectFormat(url, baseUrl);

            Assert.That(result, Is.True);
        }

        [Test]
        public void IsCorrectFormat_IncorrectUrl_ReturnsFalse()
        {
            var url = "https://www.google.com/";
            var baseUrl = "https://www.litedb.org";

            var result = _validator.IsCorrectFormat(url, baseUrl);

            Assert.That(result, Is.False);
        }

        [Test]
        public void IsHtmlDoc_UrlWhichRefersToHtmlDoc_ReturnsTrue()
        {
            var url = "https://www.litedb.org/docs";
            var baseUrl = "https://www.litedb.org";

            var result = _validator.IsHtmlDoc(url, baseUrl);

            Assert.That(result, Is.True);
        }

        [Test]
        public void IsHtmlDoc_UrlWhichNotRefersToHtmlDoc_ReturnsFalse()
        {
            var url = "https://upload.wikimedia.org/wikipedia/commons/0/02/SVG_logo.svg";
            var baseUrl = "https://upload.wikimedia.org";

            var result = _validator.IsHtmlDoc(url, baseUrl);

            Assert.That(result, Is.False);
        }
    }
}