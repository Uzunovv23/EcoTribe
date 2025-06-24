using EcoTribe.Web.Models;
using EcoTribe.Web.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Diagnostics;

namespace EcoTribe.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public HomeController(ILogger<HomeController> logger, IStringLocalizer<SharedResource> localizer)
        {
            _logger = logger;
            _localizer = localizer;
        }

        public IActionResult Test()
        {
            var value = _localizer["GLOBAL_MENU_HOME"];
            var culture = System.Globalization.CultureInfo.CurrentUICulture.Name;

            return Content($"Culture: {culture} | GLOBAL_MENU_HOME = {value}");
        }


        public IActionResult Index()
        {
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
