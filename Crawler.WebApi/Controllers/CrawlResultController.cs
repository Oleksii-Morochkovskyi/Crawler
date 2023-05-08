using Crawler.Services;
using Crawler.WebApp.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Crawler.WebApi.Controllers
{
    [Route("api/crawlResult")]
    [ApiController]
    public class CrawlResultController : ControllerBase
    {
        private readonly DatabaseInteractionService _databaseInteractionService;
        private readonly MapModelsHelper _mapModelsHelper;

        public CrawlResultController(
            DatabaseInteractionService databaseInteractionService,
            MapModelsHelper mapModelsHelper)
        {
            _databaseInteractionService = databaseInteractionService;
            _mapModelsHelper = mapModelsHelper;
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var foundUrls = await _databaseInteractionService.GetUrlsByInitialUrlIdAsync(id);

            if (foundUrls.IsNullOrEmpty())
            {
                return NotFound(new { message = "Crawl result was not found" });
            }

            var result = _mapModelsHelper.GetResultViewModel(foundUrls);

            return new JsonResult(result);
        }
    }
}
