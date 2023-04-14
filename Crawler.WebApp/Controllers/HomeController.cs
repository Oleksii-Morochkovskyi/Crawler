using Crawler.Logic.Validators;
using Crawler.Persistence.Interfaces;
using Crawler.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Crawler.Services;

namespace Crawler.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly UrlValidator _validator;
        private readonly IFoundUrlRepository _foundUrlRepository;
        private readonly IInitialUrlRepository _initialUrlRepository;
        private readonly DatabaseInteraction _databaseInteraction;
        private readonly FoundUrlViewModel _foundUrlViewModel;
        private readonly InitialUrlViewModel _initialUrlViewModel;
        private readonly ResultViewModel _resultViewModel;

        public HomeController(
            UrlValidator validator,
            IFoundUrlRepository foundUrlRepository,
            IInitialUrlRepository initialUrlRepository,
            DatabaseInteraction databaseInteraction,
            FoundUrlViewModel foundUrlViewModel,
            InitialUrlViewModel initialUrlViewModel,
            ResultViewModel resultViewModel)
        {
            _validator = validator;
            _foundUrlRepository = foundUrlRepository;
            _initialUrlRepository = initialUrlRepository;
            _databaseInteraction = databaseInteraction;
            _foundUrlViewModel = foundUrlViewModel;
            _initialUrlViewModel = initialUrlViewModel;
            _resultViewModel = resultViewModel;
        }

        public async Task<IActionResult> Index()
        {
            var initialUrls = await _initialUrlRepository.GetInitialUrlsAsync();

            var viewModel = _initialUrlViewModel.MapInitialUrls(initialUrls);

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Submit()
        {
            var input = Request.Form["Url"];

            if (_validator.IsValidUrl(input))
            {
                var initialUrlId = await _databaseInteraction.AddUrlsAsync(input);

                return RedirectToAction("CrawlResult", new { id = initialUrlId });
            }

            TempData["ErrorMessage"] = "Invalid Url. Please try again.";

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> CrawlResult(int id)
        {
            var foundUrls = await _foundUrlRepository.GetUrlsByInitialUrlIdAsync(id);

            var viewModel = _foundUrlViewModel.MapFoundUrls(foundUrls);

            var result = _resultViewModel.GetResultViewModel(viewModel);

            return View(result);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}