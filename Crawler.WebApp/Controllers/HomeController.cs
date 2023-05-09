using Crawler.Logic.Validators;
using Crawler.Services.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Crawler.Services.Helpers;
using Crawler.Services.Services;

namespace Crawler.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly UrlValidator _validator;
        private readonly DatabaseInteractionService _databaseInteractionService;
        private readonly MapModelsHelper _mapModelsHelper;

        public HomeController(
            UrlValidator validator,
            DatabaseInteractionService databaseInteractionService,
            MapModelsHelper mapModelsHelper)
        {
            _validator = validator;
            _databaseInteractionService = databaseInteractionService;
            _mapModelsHelper = mapModelsHelper;
        }

        public async Task<IActionResult> Index()
        {
            var initialUrls = await _databaseInteractionService.GetInitialUrlsAsync();

            var viewModel = _mapModelsHelper.MapInitialUrls(initialUrls);

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

            var result = _mapModelsHelper.GetResultViewModel(foundUrls);

            return View(result);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}