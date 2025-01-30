using Microsoft.AspNetCore.Mvc;

namespace EcoTribe.Web.Models
{
    public class EventController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
