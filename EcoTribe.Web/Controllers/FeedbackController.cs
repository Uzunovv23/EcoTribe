using EcoTribe.BusinessObjects.ViewModels;
using EcoTribe.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

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
            var feedbacks = feedbackService.GetAll().ToList();

            if (feedbacks == null)
            {
                return View(new List<FeedbackViewModel>()); // Pass an empty list to avoid null issues
            }

            return View(feedbacks);
        }
    }
}
