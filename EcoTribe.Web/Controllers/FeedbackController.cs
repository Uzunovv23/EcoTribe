using EcoTribe.BusinessObjects.InputModels;
using EcoTribe.BusinessObjects.ViewModels;
using EcoTribe.Services.Implementations;
using EcoTribe.Services.Interfaces;
using EcoTribe.Services.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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
        public IActionResult Create()
        {
            var events = feedbackService.GetAllEvents();
            var volunteers = feedbackService.GetAllVolunteers();

            ViewBag.Events = new SelectList(events, "Id", "Name");
            ViewBag.Volunteers = new SelectList(volunteers, "Id", "Name");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(FeedbackInputModel inputModel)
        {
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
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, FeedbackInputModel inputModel)
        {
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
    }
}
