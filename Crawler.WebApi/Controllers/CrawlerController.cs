using Crawler.Application.Validators;
using Crawler.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Crawler.WebApi.Controllers
{
    public class CrawlerController : BaseApiController
    {
        private readonly UrlValidator _validator;
        private readonly DatabaseInteractionService _databaseInteractionService;

        public CrawlerController(
            UrlValidator validator,
            DatabaseInteractionService databaseInteractionService)
        {
            _validator = validator;
            _databaseInteractionService = databaseInteractionService;
        }

        [HttpPost]
        public async Task<IActionResult> Post(string? uriString)
        {

            if (!_validator.IsValidUrl(uriString))
            {
                return BadRequest(new { message = "You entered wrong Url" });
            }

            var initialUrlId = await _databaseInteractionService.AddUrlsAsync(uriString);

            return new JsonResult(initialUrlId);
        }
    }
}