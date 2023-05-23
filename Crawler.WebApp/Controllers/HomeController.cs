using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Crawler.Application.Mappers;
using Crawler.Application.Validators;
using Crawler.Application.Services;
using Crawler.Domain.ViewModels;

namespace Crawler.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly UrlValidator _validator;
        private readonly DatabaseInteractionService _databaseInteractionService;
        private readonly ModelMapper _modelMapper;

        public HomeController(
            UrlValidator validator,
            DatabaseInteractionService databaseInteractionService,
            ModelMapper modelMapper)
        {
            _validator = validator;
            _databaseInteractionService = databaseInteractionService;
            _modelMapper = modelMapper;
        }

        public async Task<IActionResult> Index()
        {
            var initialUrls = await _databaseInteractionService.GetInitialUrlsAsync();

            var viewModel = _modelMapper.MapInitialUrls(initialUrls);

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Submit()
        {
            var input = Request.Form["Url"];

            if (_validator.IsValidUrl(input))
            {
                var initialUrlId = await _databaseInteractionService.AddUrlsAsync(input);

                return RedirectToAction("CrawlResult", new { id = initialUrlId });
            }

            TempData["ErrorMessage"] = "Invalid Url. Please try again.";

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> CrawlResult(int id)
        {
            var foundUrls = await _databaseInteractionService.GetUrlsByInitialUrlIdAsync(id);

            var result = _modelMapper.GetResultViewModel(foundUrls);

            return View(result);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}