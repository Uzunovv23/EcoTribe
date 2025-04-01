using EcoTribe.BusinessObjects.Domain.Models;
using EcoTribe.BusinessObjects.InputModels;
using EcoTribe.BusinessObjects.ViewModels;
using EcoTribe.Services.Implementations;
using EcoTribe.Services.Interfaces;
using EcoTribe.Services.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace EcoTribe.Web.Controllers
{
    public class EventController : Controller
    {
        private readonly IEventService eventService;
        private readonly IVolunteerService volunteerService;

        public EventController(IEventService eventService, IVolunteerService volunteerService)
        {
            this.eventService = eventService;
            this.volunteerService = volunteerService;
        }
        public IActionResult Index()
        {
            List<EventViewModel> events = eventService.GetAll().ToList();
            return View(events);
        }

        [Authorize(Roles = "Administrator, Organizator")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Administrator, Organizator")]
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

        public IActionResult Details(int id)
        {
            var viewModel = eventService.GetByIdWithVolunteersAndSponsorsAndFeedbacks(id);
            if (viewModel == null)
            {
                return NotFound();
            }

            string? userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            string? userRole = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;

            viewModel.UserId = userId;
            viewModel.UserRole = userRole;

            if (userRole == "User" && !string.IsNullOrEmpty(userId))
            {
                // Fetch the volunteer entity (not a ViewModel)
                var volunteer = volunteerService.GetByUserId(userId);
                viewModel.VolunteerId = volunteer?.Id; // Use only the ID, discard the rest
            }
            else
            {
                viewModel.VolunteerId = null;
            }

            viewModel.FeedbackInput = new FeedbackInputModel
            {
                EventId = id,
                // ApplicationUserId = userId,
                VolunteerId = viewModel.VolunteerId ?? 0,
                CreatedAt = DateTime.UtcNow
            };

            return View(viewModel);
        }


        [Authorize(Roles = "Administrator, Organizator")]
        public IActionResult AddSponsor(int eventId)
        {
            var eventEntity = eventService.GetById(eventId);
            if (eventEntity == null)
            {
                return NotFound();
            }

            var organizations = eventService.GetOrganizations();

            ViewBag.Organizations = new SelectList(organizations, "Id", "Name");

            var model = new AddSponsorInputModel
            {
                EventId = eventId
            };

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator, Organizator")]
        [ValidateAntiForgeryToken]
        public IActionResult AddSponsor(AddSponsorInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                eventService.AddSponsor(model.EventId, model.OrganizationId);
                return RedirectToAction("Details", new { id = model.EventId });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while adding the sponsor.");
                return View(model);
            }
        }

    }
}
