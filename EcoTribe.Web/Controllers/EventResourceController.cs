using EcoTribe.BusinessObjects.ViewModels;
using EcoTribe.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EcoTribe.Web.Controllers
{
    public class EventResourceController : Controller
    {
        private readonly IEventResourceService eventResourceService;

        public EventResourceController(IEventResourceService eventResourceService)
        {
            this.eventResourceService = eventResourceService;
        }
        public IActionResult Index()
        {
            List<EventResourceViewModel> eventResources = eventResourceService.GetAll().ToList();
            return(View(eventResources));
        }
    }
}
