using EcoTribe.BusinessObjects.InputModels;
using EcoTribe.BusinessObjects.ViewModels;
using EcoTribe.Services.Interfaces;
using EcoTribe.Services.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcoTribe.Web.Controllers
{
    public class EventController : Controller
    {
        private readonly IEventService eventService;

        public EventController(IEventService eventService)
        {
            this.eventService = eventService;
        }
        public IActionResult Index()
        {
            List<EventViewModel> events = eventService.GetAll().ToList();
            return View(events);
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(EventInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                model.Start = DateTime.SpecifyKind(model.Start, DateTimeKind.Utc);
                model.End = DateTime.SpecifyKind(model.End, DateTimeKind.Utc);

                eventService.Create(model);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while saving the event.");
                return View(model);
            }
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Edit(int id)
        {
            var eventEntity = eventService.GetById(id);
            if (eventEntity == null)
            {
                return NotFound();
            }

            var inputModel = ModelConverter.ConvertToModel<EventViewModel, EventInputModel>(eventEntity);
            return View(inputModel);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, EventInputModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                return View(inputModel);
            }
            try
            {
                inputModel.Start = DateTime.SpecifyKind(inputModel.Start, DateTimeKind.Utc);
                inputModel.End = DateTime.SpecifyKind(inputModel.End, DateTimeKind.Utc);
                eventService.Update(id, inputModel);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while saving the event.");
                return View(inputModel);
            }
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Delete(int id)
        {
            var eventEntity = eventService.GetById(id);
            if (eventEntity == null)
            {
                return NotFound();
            }

            return View(eventEntity); 
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                eventService.Delete(id);
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest("Error deleting the event.");
            }
        }
    }
}
