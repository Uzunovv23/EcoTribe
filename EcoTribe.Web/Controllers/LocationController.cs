using EcoTribe.BusinessObjects.InputModels;
using EcoTribe.BusinessObjects.ViewModels;
using EcoTribe.Services.Implementations;
using EcoTribe.Services.Interfaces;
using EcoTribe.Services.Utils;
using Microsoft.AspNetCore.Mvc;

namespace EcoTribe.Web.Controllers
{
    public class LocationController : Controller
    {
        private readonly ILocationService locationService;

        public LocationController(ILocationService locationService)
        {
            this.locationService = locationService;
        }
        public IActionResult Index()
        {
            List<LocationViewModel> locations = locationService.GetAll().ToList();
            return View(locations);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(LocationInputModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                return View(inputModel);
            }
            try
            {
                locationService.Create(inputModel); // Call the service method
                return RedirectToAction(nameof(Index)); // Redirect back to locations list
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "An error occurred while saving the location.");
                return View(inputModel);
            }
        }
        public IActionResult Edit(int id)
        {
            var location = locationService.GetById(id);
            if (location == null)
            {
                return NotFound();
            }

            var inputModel = ModelConverter.ConvertToModel<LocationViewModel, LocationInputModel>(location);

            return View(inputModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, LocationInputModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                return View(inputModel);
            }

            try
            {
                locationService.Update(id, inputModel);
                return RedirectToAction(nameof(Index)); 
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while updating the location.");
                return View(inputModel);
            }
        }
    }
}
