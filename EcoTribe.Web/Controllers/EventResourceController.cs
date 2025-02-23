using EcoTribe.BusinessObjects.InputModels;
using EcoTribe.BusinessObjects.ViewModels;
using EcoTribe.Services.Interfaces;
using EcoTribe.Services.Utils;
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

        public IActionResult Edit(int id)
        {
            var eventResourceEntity = eventResourceService.GetById(id);
            if (eventResourceEntity == null)
            {
                return NotFound();
            }
            var inputModel = ModelConverter.ConvertToModel<EventResourceViewModel, EventResourceInputModel>(eventResourceEntity);
            return View(inputModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, EventResourceInputModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                return View(inputModel);
            }
            try
            {
                eventResourceService.Update(id, inputModel);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while saving the event resource.");
                return View(inputModel);
            }
        }

        public IActionResult Delete(int id)
        {
            var eventResource = eventResourceService.GetById(id);
            if (eventResource == null)
            {
                return NotFound();
            }
            return View(eventResource);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                eventResourceService.Delete(id);
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest("Error deleting the event resource.");
            }
        }
    }
}
