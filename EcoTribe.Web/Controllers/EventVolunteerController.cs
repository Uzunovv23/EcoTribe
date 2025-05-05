using EcoTribe.BusinessObjects.InputModels;
using EcoTribe.BusinessObjects.ViewModels;
using EcoTribe.Services.Implementations;
using EcoTribe.Services.Interfaces;
using EcoTribe.Services.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;

namespace EcoTribe.Web.Controllers
{
    public class EventVolunteerController : Controller
    {
        private readonly IEventVolunteerService eventVolunteerService;
        private readonly IVolunteerService volunteerService;

        public EventVolunteerController(
            IEventVolunteerService eventVolunteerService,
            IVolunteerService volunteerService)
        {
            this.eventVolunteerService = eventVolunteerService;
            this.volunteerService = volunteerService;
        }
        public IActionResult Index()
        {
            List<EventVolunteerViewModel> eventVolunteers = eventVolunteerService.GetAll().ToList();
            return View(eventVolunteers);
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(EventVolunteerInputModel inputModel)
        {
            inputModel.ApplicationUserId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            ModelState.Remove("ApplicationUserId");
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

        [Authorize(Roles = "Administrator")]
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
        [Authorize(Roles = "Administrator")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, EventVolunteerInputModel inputModel)
        {
            inputModel.ApplicationUserId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            ModelState.Remove("ApplicationUserId");
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

        [Authorize(Roles = "Administrator")]
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
        [Authorize(Roles = "Administrator")]
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

        [HttpPost]
        [Authorize(Roles = "User")]
        [ValidateAntiForgeryToken]
        public IActionResult ToggleParticipation([FromForm] ParticipateInputModel model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var volunteer = volunteerService.GetByUserId(userId);

            if (volunteer == null)
            {
                return Unauthorized();
            }

            bool alreadyJoined = eventVolunteerService.HasUserAlreadyParticipated(model.EventId, volunteer.Id);

            try
            {
                if (alreadyJoined)
                {
                    eventVolunteerService.Unparticipate(model.EventId, volunteer.Id);
                    return Ok(new
                    {
                        isParticipating = false,
                        message = "You have successfully canceled your participation."
                    });
                }
                else
                {
                    eventVolunteerService.Participate(model.EventId, volunteer.Id);
                    return Ok(new
                    {
                        isParticipating = true,
                        message = "You have successfully signed up!"
                    });
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }


    }
}
