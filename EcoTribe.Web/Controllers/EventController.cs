using EcoTribe.BusinessObjects.Domain.Enums;
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
        private readonly IEventVolunteerService eventVolunteerService;
        private readonly IOrganizationService organizationService;
        private readonly IModelConverter modelConverter;

        public EventController(IEventService eventService, IVolunteerService volunteerService, IEventVolunteerService eventVolunteerService, IOrganizationService organizationService, IModelConverter modelConverter)
        {
            this.eventService = eventService;
            this.volunteerService = volunteerService;
            this.eventVolunteerService = eventVolunteerService;
            this.organizationService = organizationService;
            this.modelConverter = modelConverter;
        }
        public IActionResult Index()
        {
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            OrganizationViewModel? organization = organizationService.GetByUserId(userId);

            if (organization != null && organization.Status != OrganizationStatus.Approved)
            {
                organization = null;  
            }

            List<EventViewModel> events = eventService.GetAll(userId).ToList();

            EventsViewModel eventsViewModel = new EventsViewModel
            {
                Events = events,
                UserApprovedOrganization = organization  
            };

            return View(eventsViewModel);
        }


        [Authorize(Roles = "Administrator, Organizator")]
        public IActionResult Create()
        {
            OrganizationViewModel? organization = organizationService.GetByUserId(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            if (organization != null && organization.Status != OrganizationStatus.Approved)
            {
                return Unauthorized();
            }

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Administrator, Organizator")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(EventInputModel model)
        {
            OrganizationViewModel? organization = organizationService.GetByUserId(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            if (organization != null && organization.Status != OrganizationStatus.Approved)
            {
                return Unauthorized();
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                string? userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (userIdString == null)
                {
                    return Unauthorized();
                }



                model.CreatedBy = organizationService.GetByUserId(userIdString).Id;

                model.Start = DateTime.SpecifyKind(model.Start, DateTimeKind.Utc);
                model.End = DateTime.SpecifyKind(model.End, DateTimeKind.Utc);

                eventService.CreateAndNotifyUsers(model);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
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

            var inputModel = modelConverter.ConvertToModel<EventViewModel, EventInputModel>(eventEntity);
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

            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            string? userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            viewModel.UserId = userId;
            viewModel.UserRole = userRole;

            if (userRole == "User" && !string.IsNullOrEmpty(userId))
            {
                Volunteer volunteer = volunteerService.GetByUserId(userId);

                if (volunteer != null)
                {
                    viewModel.VolunteerId = volunteer.Id;

                    bool isParticipating = eventVolunteerService.HasUserAlreadyParticipated(id, volunteer.Id);
                    viewModel.IsParticipating = isParticipating;

                    if (Request.Query.ContainsKey("toggle"))
                    {
                        if (isParticipating)
                        {
                            eventVolunteerService.Unparticipate(id, volunteer.Id);
                        }
                        else
                        {
                            eventVolunteerService.Participate(id, volunteer.Id);
                        }

                        return RedirectToAction(nameof(Details), new { id });
                    }

                    viewModel.FeedbackInput = new FeedbackInputModel
                    {
                        EventId = id,
                        VolunteerId = volunteer.Id,
                        CreatedAt = DateTime.UtcNow,
                        VolunteerName = volunteer.Name
                    };
                }
                else
                {
                    viewModel.VolunteerId = null;
                    viewModel.IsParticipating = false;
                }
            }
            else
            {
                viewModel.VolunteerId = null;
                viewModel.FeedbackInput = null;
                viewModel.IsParticipating = false;
            }

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

        [Authorize(Roles = "Administrator, Organizator")]
        public IActionResult AddPhoto([FromRoute(Name = "id")] int eventId)
        {
            return View(new AddPhotoInputModel { EventId = eventId });
        }

        [HttpPost]
        [Authorize(Roles = "Administrator, Organizator")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPhoto(AddPhotoInputModel model)
        {
            if (model.Photo == null || model.Photo.Length == 0)
            {
                ModelState.AddModelError("", "Please upload a valid photo.");
                return View(model);
            }

            try
            {
                await eventService.AddPhotoAsync(model.EventId, model.Photo);
                return RedirectToAction("Details", new { id = model.EventId });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while uploading the photo.");
                return View(model);
            }
        }
    }
}
