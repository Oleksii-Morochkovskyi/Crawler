using Crawler.Utils.Mappers;
using Crawler.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Crawler.WebApi.Controllers
{
    public class ResultController : BaseApiController
    {
        private readonly DatabaseInteractionService _databaseInteractionService;
        private readonly ModelMapper _modelMapper;

        public ResultController(
            DatabaseInteractionService databaseInteractionService,
            ModelMapper modelMapper)
        {
            _databaseInteractionService = databaseInteractionService;
            _modelMapper = modelMapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var initialUrls = await _databaseInteractionService.GetInitialUrlsAsync();

            var viewModel = _modelMapper.MapInitialUrls(initialUrls);

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

            var result = _modelMapper.GetResultViewModel(foundUrls);

            return new JsonResult(result);
        }
    }
}
