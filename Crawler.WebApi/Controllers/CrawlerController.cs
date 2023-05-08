using Crawler.Logic.Validators;
using Crawler.Services;
using Crawler.WebApp.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace Crawler.WebApi.Controllers
{
    [ApiController]
    [Route("api/crawler")]
    public class CrawlerController : ControllerBase
    {
        private readonly UrlValidator _validator;
        private readonly DatabaseInteractionService _databaseInteractionService;
        private readonly MapModelsHelper _mapModelsHelper;

        public CrawlerController(
            UrlValidator validator,
            DatabaseInteractionService databaseInteractionService,
            MapModelsHelper mapModelsHelper)
        {
            _validator = validator;
            _databaseInteractionService = databaseInteractionService;
            _mapModelsHelper = mapModelsHelper;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var initialUrls = await _databaseInteractionService.GetInitialUrlsAsync();

            var viewModel = _mapModelsHelper.MapInitialUrls(initialUrls);

            return new JsonResult(viewModel);
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