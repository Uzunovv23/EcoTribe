using EcoTribe.BusinessObjects.InputModels;
using EcoTribe.BusinessObjects.ViewModels;
using EcoTribe.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace EcoTribe.Web.Controllers
{
    public class VolunteerController : Controller
    {
        private readonly IVolunteerService volunteerService;

        public VolunteerController(IVolunteerService volunteerService)
        {
            this.volunteerService = volunteerService;
        }
        public IActionResult Index()
        {
            List<VolunteerViewModel> volunteers = volunteerService.GetAll().ToList();
            return View(volunteers);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(VolunteerInputModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                return View(inputModel);
            }
            try
            {
                inputModel.Latitude = Convert.ToDecimal(inputModel.Latitude, CultureInfo.InvariantCulture);
                inputModel.Longitude = Convert.ToDecimal(inputModel.Longitude, CultureInfo.InvariantCulture);

                volunteerService.Create(inputModel);
                return RedirectToAction(nameof(Index)); 
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while saving the volunteer.");
                return View(inputModel);
            }
        }
    }
}
