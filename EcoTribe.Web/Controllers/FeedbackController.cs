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
            List<FeedbackViewModel> feedbacks = feedbackService.GetAll().ToList();
            return View(feedbacks);
        }
    }
}
