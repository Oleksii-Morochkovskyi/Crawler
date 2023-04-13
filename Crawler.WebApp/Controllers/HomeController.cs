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
        private readonly Crawler.Logic.Crawlers.Crawler _crawler;
        private readonly UrlValidator _validator;
        private readonly IFoundUrlRepository _foundUrlRepository;
        private readonly IInitialUrlRepository _initialUrlRepository;
        private readonly DatabaseInteraction _databaseInteraction;

        public HomeController(ILogger<HomeController> logger, UrlValidator validator, Logic.Crawlers.Crawler crawler, IFoundUrlRepository foundUrlRepository, IInitialUrlRepository initialUrlRepository, DatabaseInteraction databaseInteraction)
        {
            _logger = logger;
            _crawler = crawler;
            _validator = validator;
            _foundUrlRepository = foundUrlRepository;
            _initialUrlRepository = initialUrlRepository;
            _databaseInteraction = databaseInteraction;
        }

        public IActionResult Input()
        {
            var initialUrls = _initialUrlRepository.GetInitialUrls();

            return View((initialUrls));
        }

        [HttpPost]
        public IActionResult Submit()
        {
            var input = Request.Form["Url"];

            if (!_validator.IsValidUrl(input))
            {
                TempData["ErrorMessage"] = "Invalid Url. Please try again.";
                return RedirectToAction("Input");
            }

            return RedirectToAction("Crawl", new { url = input });
        }

        public async Task<IActionResult> Crawl(string url)
        {
            var initialUrl = url.TrimEnd('/');

            var result = await _crawler.StartCrawlerAsync(initialUrl);

            var initialUrlId = await _databaseInteraction.AddUrlsAsync(result, initialUrl);

            return RedirectToAction("Result", new { id = initialUrlId });
        }
    
        public IActionResult Result(int id)
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