using System.Text.Json.Nodes;
using Crawler.Logic.Validators;
using Crawler.Utils.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

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
        public async Task<IActionResult> Post([FromBody] JsonObject body)
        {
            var url = body["url"].ToString();

            if (!_validator.IsValidUrl(url))
            {
                return BadRequest(new { message = "You entered wrong Url" });
            }

            var initialUrlId = await _databaseInteractionService.AddUrlsAsync(url);

            return new JsonResult(initialUrlId);
        }
    }
}