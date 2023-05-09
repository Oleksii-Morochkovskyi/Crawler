using Crawler.Logic.Validators;
using Crawler.Services.Services;
using Microsoft.AspNetCore.Mvc;

namespace Crawler.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CrawlerController : ControllerBase
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
        public async Task<IActionResult> Post([FromBody] string url)
        {
            if (!_validator.IsValidUrl(url))
            {
                return BadRequest(new { message = "You entered wrong Url" });
            }

            var initialUrlId = await _databaseInteractionService.AddUrlsAsync(url);

            return new JsonResult(initialUrlId);
        }
    }
}