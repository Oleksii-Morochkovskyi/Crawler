using Crawler.Logic.Validators;
using Crawler.Persistence.Interfaces;
using Crawler.Services;
using Crawler.WebApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace Crawler.WebApi.Controllers
{
    [ApiController]
    [Route("api/crawler")]
    public class CrawlerController : ControllerBase
    {
        private readonly UrlValidator _validator;
        private readonly DatabaseInteractionService _databaseInteractionService;
        private readonly FoundUrlViewModel _foundUrlViewModel;
        private readonly InitialUrlViewModel _initialUrlViewModel;
        private readonly ResultViewModel _resultViewModel;

        public CrawlerController(
            UrlValidator validator,
            DatabaseInteractionService databaseInteractionService,
            FoundUrlViewModel foundUrlViewModel,
            InitialUrlViewModel initialUrlViewModel,
            ResultViewModel resultViewModel)
        {
            _validator = validator;
            _databaseInteractionService = databaseInteractionService;
            _foundUrlViewModel = foundUrlViewModel;
            _initialUrlViewModel = initialUrlViewModel;
            _resultViewModel = resultViewModel;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var initialUrls = await _databaseInteractionService.GetInitialUrlsAsync();

            var viewModel = _initialUrlViewModel.MapInitialUrls(initialUrls);

            return new ObjectResult(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] string url)
        {
            if (!_validator.IsValidUrl(url))
            {
                return BadRequest();
            }

            var initialUrlId = await _databaseInteractionService.AddUrlsAsync(url);

            return new ObjectResult(initialUrlId);
            
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var foundUrls = await _databaseInteractionService.GetUrlsByInitialUrlIdAsync(id);

            var viewModel = _foundUrlViewModel.MapFoundUrls(foundUrls);

            var result = _resultViewModel.GetResultViewModel(viewModel);

            return new ObjectResult(result);
        }
    }
}