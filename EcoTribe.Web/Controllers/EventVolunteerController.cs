using EcoTribe.BusinessObjects.InputModels;
using EcoTribe.BusinessObjects.ViewModels;
using EcoTribe.Services.Interfaces;
using EcoTribe.Services.Utils;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certificates;

namespace EcoTribe.Web.Controllers
{
    public class EventVolunteerController : Controller
    {
        private readonly IEventVolunteerService eventVolunteerService;

        public EventVolunteerController(IEventVolunteerService eventVolunteerService)
        {
            this.eventVolunteerService = eventVolunteerService;
        }
        public IActionResult Index()
        {
            List<EventVolunteerViewModel> eventVolunteers = eventVolunteerService.GetAll().ToList();
            return View(eventVolunteers);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(EventVolunteerInputModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                return View(inputModel);
            }
            try
            {
                eventVolunteerService.Create(inputModel);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "An error occurred while saving the event volunteer.");
                return View(inputModel);
            }
        }
        public IActionResult Edit(int id)
        {
            var eventVolunteer = eventVolunteerService.GetById(id);
            if (eventVolunteer == null)
            {
                return NotFound();
            }
            var inputModel = ModelConverter.ConvertToModel<EventVolunteerViewModel, EventVolunteerInputModel>(eventVolunteer);
            return View(inputModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, EventVolunteerInputModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                return View(inputModel);
            }
            try
            {
                eventVolunteerService.Update(id, inputModel);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "An error occurred while updating the event volunteer.");
                return View(inputModel);
            }
        }
        public IActionResult Delete(int id)
        {
            var eventVolunteer = eventVolunteerService.GetById(id);
            if (eventVolunteer == null)
            {
                return NotFound();
            }

            return View(eventVolunteer);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                eventVolunteerService.Delete(id);
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest("Error deleting the event volunteer.");
            }
        }
    }
}
