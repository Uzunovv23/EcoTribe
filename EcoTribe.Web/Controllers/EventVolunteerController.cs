using EcoTribe.BusinessObjects.ViewModels;
using EcoTribe.Services.Interfaces;
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
    }
}
