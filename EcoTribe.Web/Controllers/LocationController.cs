using EcoTribe.BusinessObjects.InputModels;
using EcoTribe.BusinessObjects.ViewModels;
using EcoTribe.Services.Implementations;
using EcoTribe.Services.Interfaces;
using EcoTribe.Services.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcoTribe.Web.Controllers
{
    public class LocationController : Controller
    {
        private readonly ILocationService locationService;
        private readonly IModelConverter modelConverter;

        public LocationController(ILocationService locationService, IModelConverter modelConverter)
        {
            this.locationService = locationService;
            this.modelConverter = modelConverter;
        }
        public IActionResult Index()
        {
            List<LocationViewModel> locations = locationService.GetAll().ToList();
            return View(locations);
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(LocationInputModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                return View(inputModel);
            }
            try
            {
                locationService.Create(inputModel); 
                return RedirectToAction(nameof(Index)); 
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "An error occurred while saving the location.");
                return View(inputModel);
            }
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Edit(int id)
        {
            var location = locationService.GetById(id);
            if (location == null)
            {
                return NotFound();
            }

            var inputModel = modelConverter.ConvertToModel<LocationViewModel, LocationInputModel>(location);

            return View(inputModel);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
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

        [Authorize(Roles = "Administrator")]
        public IActionResult Delete(int id)
        {
            var location = locationService.GetById(id);
            if (location == null)
            {
                return NotFound();
            }

            return View(location);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                locationService.Delete(id);
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest("Error deleting the location.");
            }
        }
    }
}
