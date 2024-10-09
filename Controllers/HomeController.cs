using Microsoft.AspNetCore.Mvc;
using SiteCopy.Models;
using System;
using System.Diagnostics;

namespace SiteCopy.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ScraperService _spaScraper;

        public HomeController(ILogger<HomeController> logger, ScraperService spaScraper)
        {
            _logger = logger;
            _spaScraper = spaScraper;
        }

        [HttpGet]
        public async Task<IActionResult> Clone(string url)
        {
            var outputDir = Path.Combine(Directory.GetCurrentDirectory(), "ClonedSpa");
            await _spaScraper.CloneSpaAsync(url, outputDir);
            return Ok($"SPA cloned to {outputDir}");
        }

        public async Task<IActionResult> Index()
        {

            return RedirectToAction(nameof(Clone),new { url = "https://mangadex.org" });
            //var render = new DynamicScraper().ScrapePageAsync("https://mangadex.org");
            //var httpClient = new HttpClient();
            //var scraperService = new ScraperService(httpClient);

            //string url = "https://example.com";

            //// For MVC/traditional site
            ////string staticContent = await scraperService.ScrapeAsync(url, false);
            ////Console.WriteLine("Static Content:");
            ////Console.WriteLine(staticContent);

            //// For SPA/dynamic site
            //string dynamicContent = await scraperService.ScrapeAsync(url, true);
            //Console.WriteLine("Dynamic Content:");
            //Console.WriteLine(dynamicContent);
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
