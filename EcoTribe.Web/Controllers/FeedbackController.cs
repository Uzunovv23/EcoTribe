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
    public class FeedbackController : Controller
    {
        private readonly IFeedbackService feedbackService;

        public FeedbackController(IFeedbackService feedbackService)
        {
            this.feedbackService = feedbackService;
        }
        public IActionResult Index()
        {
            List<FeedbackViewModel> feedbacks = feedbackService.GetAll().ToList();
            return View(feedbacks);
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            var events = feedbackService.GetAllEvents();
            var volunteers = feedbackService.GetAllVolunteers();

            ViewBag.Events = new SelectList(events, "Id", "Name");
            ViewBag.Volunteers = new SelectList(volunteers, "Id", "Name");

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(FeedbackInputModel inputModel)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized();
            }
            inputModel.ApplicationUserId = userId; 

            if (!ModelState.IsValid)
            {
                ViewBag.Events = new SelectList(feedbackService.GetAllEvents(), "Id", "Name");
                ViewBag.Volunteers = new SelectList(feedbackService.GetAllVolunteers(), "Id", "Name");
                return View(inputModel);
            }

            try
            {
                feedbackService.Create(inputModel);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "An error occurred while saving the feedback.");
                ViewBag.Events = new SelectList(feedbackService.GetAllEvents(), "Id", "Name");
                ViewBag.Volunteers = new SelectList(feedbackService.GetAllVolunteers(), "Id", "Name");

                return View(inputModel); 
            }
        }


        [Authorize(Roles = "Administrator")]
        public IActionResult Edit(int id)
        {
            var feedback = feedbackService.GetById(id);
            if (feedback == null)
            {
                return NotFound();
            }

            ViewBag.Events = new SelectList(feedbackService.GetAllEvents(), "Id", "Name");
            ViewBag.Volunteers = new SelectList(feedbackService.GetAllVolunteers(), "Id", "Name");

            var inputModel = ModelConverter.ConvertToModel<FeedbackViewModel, FeedbackInputModel>(feedback);
            return View(inputModel);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, FeedbackInputModel inputModel)
        {
            inputModel.ApplicationUserId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            ModelState.Remove("ApplicationUserId");
            if (!ModelState.IsValid)
            {
                ViewBag.Events = new SelectList(feedbackService.GetAllEvents(), "Id", "Name");
                ViewBag.Volunteers = new SelectList(feedbackService.GetAllVolunteers(), "Id", "Name");
                return View(inputModel);
            }

            try
            {
                feedbackService.Update(id, inputModel);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "An error occurred while updating the feedback.");
                ViewBag.Events = new SelectList(feedbackService.GetAllEvents(), "Id", "Name");
                ViewBag.Volunteers = new SelectList(feedbackService.GetAllVolunteers(), "Id", "Name");
                return View(inputModel);
            }
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Delete(int id)
        {
            var feedback = feedbackService.GetById(id);
            if (feedback == null)
            {
                return NotFound();
            }
            return View(feedback);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                feedbackService.Delete(id);
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest("Error deleting the feedback.");
            }
        }

        [HttpGet]
        [Authorize]
        public IActionResult SubmitFeedback(int eventId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }

            if (feedbackService.HasUserProvidedFeedback(eventId, userId))
            {
                TempData["ErrorMessage"] = "You have already submitted feedback for this event.";
                return RedirectToAction("Details", "Event", new { id = eventId });
            }

            var feedbackModel = new FeedbackInputModel { EventId = eventId };
            return View(feedbackModel);
        }


        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult SubmitFeedback(FeedbackInputModel inputModel)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }

            inputModel.ApplicationUserId = userId;

            if (!ModelState.IsValid)
            {
                return View(inputModel); 
            }

            if (feedbackService.HasUserProvidedFeedback(inputModel.EventId, userId))
            {
                TempData["ErrorMessage"] = "You have already submitted feedback for this event.";
                return RedirectToAction("Details", "Event", new { id = inputModel.EventId });
            }

            try
            {
                feedbackService.Create(inputModel);
                TempData["SuccessMessage"] = "Feedback submitted successfully.";
                return RedirectToAction("Details", "Event", new { id = inputModel.EventId });
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "An error occurred while submitting feedback.");
                return View(inputModel);
            }
        }

    }
}
