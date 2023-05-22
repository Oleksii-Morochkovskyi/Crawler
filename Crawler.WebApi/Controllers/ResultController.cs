using Crawler.Services.Helpers;
using Crawler.Services.Models;
using Crawler.Utils.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Crawler.WebApi.Controllers
{
    public class ResultController : BaseApiController
    {
        private readonly DatabaseInteractionService _databaseInteractionService;
        private readonly MapModelsHelper _mapModelsHelper;

        public ResultController(
            DatabaseInteractionService databaseInteractionService,
            MapModelsHelper mapModelsHelper)
        {
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
