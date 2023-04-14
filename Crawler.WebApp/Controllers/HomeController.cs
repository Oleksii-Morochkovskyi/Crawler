using Crawler.Logic.Validators;
using Crawler.Persistence.Interfaces;
using Crawler.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Crawler.ConsoleOutput;

namespace Crawler.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UrlValidator _validator;
        private readonly IFoundUrlRepository _foundUrlRepository;
        private readonly IInitialUrlRepository _initialUrlRepository;
        private readonly DatabaseInteraction _databaseInteraction;

        public HomeController(ILogger<HomeController> logger, UrlValidator validator, IFoundUrlRepository foundUrlRepository, IInitialUrlRepository initialUrlRepository, DatabaseInteraction databaseInteraction)
        {
            _logger = logger;
            _validator = validator;
            _foundUrlRepository = foundUrlRepository;
            _initialUrlRepository = initialUrlRepository;
            _databaseInteraction = databaseInteraction;
        }

        public IActionResult Start()
        {
            var initialUrls = _initialUrlRepository.GetInitialUrls();

            return View((initialUrls));
        }

        [HttpPost]
        public IActionResult Submit()
        {
            var input = Request.Form["Url"];

            if (_validator.IsValidUrl(input))
            {
                return RedirectToAction("Crawl", new { url = input });
            }

            TempData["ErrorMessage"] = "Invalid Url. Please try again.";

            return RedirectToAction("Start");
        }

        public async Task<IActionResult> Crawl(string url)
        {
            var initialUrlId = await _databaseInteraction.AddUrlsAsync(url);

            return RedirectToAction("CrawlResult", new { id = initialUrlId });
        }
    
        public IActionResult CrawlResult(int id)
        {
            var foundUrls= _foundUrlRepository.GetUrlsByInitialUrlId(id);

            return View(foundUrls);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}