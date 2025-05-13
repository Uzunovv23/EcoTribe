using EcoTribe.BusinessObjects.ViewModels;
using EcoTribe.Services.Implementations;
using EcoTribe.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EcoTribe.Web.Controllers
{
    public class NotificationController : Controller
    {
        private readonly INotificationService notificationService;

        public NotificationController(INotificationService notificationService)
        {
            this.notificationService = notificationService;
        }

        [HttpGet]
        public async Task<IActionResult> UnreadCount()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var count = await notificationService.GetUnreadCountAsync(userId);
            return Json(count);
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var notifications = await notificationService.GetUserNotificationsAsync(userId);
            return View(notifications); // Create a view for listing
        }
    }
}
