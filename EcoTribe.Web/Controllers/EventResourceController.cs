using EcoTribe.BusinessObjects.InputModels;
using EcoTribe.BusinessObjects.ViewModels;
using EcoTribe.Services.Interfaces;
using EcoTribe.Services.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcoTribe.Web.Controllers
{
    public class EventResourceController : Controller
    {
        private readonly IEventResourceService eventResourceService;
        private readonly IModelConverter modelConverter;

        public EventResourceController(IEventResourceService eventResourceService, IModelConverter modelConverter)
        {
            this.eventResourceService = eventResourceService;
            this.modelConverter = modelConverter;
        }
        public IActionResult Index()
        {
            List<EventResourceViewModel> eventResources = eventResourceService.GetAll().ToList();
            return(View(eventResources));
        }

        [Authorize(Roles = "Administrator,Organizator")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Administrator,Organizator")]
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

        [Authorize(Roles = "Administrator,Organizator")]
        public IActionResult Edit(int id)
        {
            var eventResourceEntity = eventResourceService.GetById(id);
            if (eventResourceEntity == null)
            {
                return NotFound();
            }
            var inputModel = modelConverter.ConvertToModel<EventResourceViewModel, EventResourceInputModel>(eventResourceEntity);
            return View(inputModel);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator,Organizator")]
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

        [Authorize(Roles = "Administrator,Organizator")]
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
        [Authorize(Roles = "Administrator,Organizator")]
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
