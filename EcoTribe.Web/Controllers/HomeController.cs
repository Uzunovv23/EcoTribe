using EcoTribe.Services.Interfaces;
using EcoTribe.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace EcoTribe.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IEmailService _mailService;

        public HomeController(ILogger<HomeController> logger, IEmailService emailService)
        {
            _logger = logger;
            _mailService = emailService;
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
