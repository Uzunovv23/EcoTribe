using EcoTribe.BusinessObjects.InputModels;
using EcoTribe.BusinessObjects.ViewModels;
using EcoTribe.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EcoTribe.Web.Controllers
{
    public class EventResourceController : Controller
    {
        private readonly IEventResourceService eventResourceService;

        public EventResourceController(IEventResourceService eventResourceService)
        {
            this.eventResourceService = eventResourceService;
        }
        public IActionResult Index()
        {
            List<EventResourceViewModel> eventResources = eventResourceService.GetAll().ToList();
            return(View(eventResources));
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(EventResourceInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                eventResourceService.Create(model);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while saving the event resource.");
                return View(model);
            }
        }

    }
}
