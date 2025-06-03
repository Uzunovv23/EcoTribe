using EcoTribe.BusinessObjects.InputModels;
using EcoTribe.BusinessObjects.ViewModels;
using EcoTribe.Services.Interfaces;
using EcoTribe.Services.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EcoTribe.Web.Controllers
{
    public class OrganizationController : Controller
    {
        private readonly IOrganizationService organizationService;

        public OrganizationController(IOrganizationService organizationService)
        {
            this.organizationService = organizationService;
        }

        public IActionResult Index()
        {
            List<OrganizationViewModel> organizations = organizationService.GetAll().ToList();
            return View(organizations);
        }

        [Authorize(Roles = "Administrator , Organizator")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Administrator , Organizator" )]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrganizationInputModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                return View(inputModel);
            }

            try
            {
                var user = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user == null)
                {
                    return Unauthorized();
                }

                await organizationService.CreateAsync(inputModel, user);  
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "An error occurred while saving the organization.");
                return View(inputModel);
            }
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Edit(int id)
        {
            var organization = organizationService.GetById(id);
            if (organization == null)
            {
                return NotFound();
            }

            var inputModel = ModelConverter.ConvertToModel<OrganizationViewModel, OrganizationInputModel>(organization);

            return View(inputModel);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, OrganizationInputModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                return View(inputModel);
            }

            try
            {
                organizationService.Update(id, inputModel);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while updating the organization.");
                return View(inputModel);
            }
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Delete(int id)
        {
            var organization = organizationService.GetById(id);
            if (organization == null)
            {
                return NotFound();
            }

            return View(organization);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                organizationService.Delete(id);
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest("Error deleting the organization.");
            }
        }


    }
}
