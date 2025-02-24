using EcoTribe.BusinessObjects.InputModels;
using EcoTribe.BusinessObjects.ViewModels;
using EcoTribe.Services.Interfaces;
using EcoTribe.Services.Utils;
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

        public IActionResult Edit(int id)
        {
            var volunteer = volunteerService.GetById(id);
            if (volunteer == null)
            {
                return NotFound();
            }
            var inputModel = ModelConverter.ConvertToModel<VolunteerViewModel, VolunteerInputModel>(volunteer);
            return View(inputModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, VolunteerInputModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                return View(inputModel);
            }
            try
            {
                inputModel.Latitude = Convert.ToDecimal(inputModel.Latitude, CultureInfo.InvariantCulture);
                inputModel.Longitude = Convert.ToDecimal(inputModel.Longitude, CultureInfo.InvariantCulture);
                volunteerService.Update(id, inputModel);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while updating the volunteer.");
                return View(inputModel);
            }
        }
        public IActionResult Delete(int id)
        {
            var volunteer = volunteerService.GetById(id);
            if (volunteer == null)
            {
                return NotFound();
            }
            return View(volunteer);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                volunteerService.Delete(id);
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest("Error deleting the volunteer.");
            }
        }
    }
}
