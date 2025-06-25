using EcoTribe.BusinessObjects.InputModels;
using EcoTribe.BusinessObjects.ViewModels;
using EcoTribe.Services.Interfaces;
using EcoTribe.Services.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace EcoTribe.Web.Controllers
{
    public class VolunteerController : Controller
    {
        private readonly IVolunteerService volunteerService;
        private readonly IModelConverter modelConverter;

        public VolunteerController(IVolunteerService volunteerService, IModelConverter modelConverter)
        {
            this.volunteerService = volunteerService;
            this.modelConverter = modelConverter;
        }
        public IActionResult Index()
        {
            List<VolunteerViewModel> volunteers = volunteerService.GetAll().ToList();
            return View(volunteers);
        }

        [Authorize(Roles= "Administrator")]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(VolunteerInputModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                return View(inputModel);
            }
            try
            {

                volunteerService.Create(inputModel);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while saving the volunteer.");
                return View(inputModel);
            }
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Edit(int id)
        {
            var volunteer = volunteerService.GetById(id);
            if (volunteer == null)
            {
                return NotFound();
            }
            var inputModel = modelConverter.ConvertToModel<VolunteerViewModel, VolunteerInputModel>(volunteer);
            return View(inputModel);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, VolunteerInputModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                return View(inputModel);
            }
            try
            {
                volunteerService.Update(id, inputModel);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while updating the volunteer.");
                return View(inputModel);
            }
        }

        [Authorize(Roles = "Administrator")]
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
        [Authorize(Roles = "Administrator")]
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
