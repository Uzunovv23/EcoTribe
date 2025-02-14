using EcoTribe.BusinessObjects.InputModels;
using EcoTribe.BusinessObjects.ViewModels;
using EcoTribe.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(OrganizationInputModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                return View(inputModel);
            }

            try
            {
                organizationService.Create(inputModel);
                return RedirectToAction(nameof(Index)); // Redirect to list of organizations
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while saving the organization.");
                return View(inputModel);
            }
        }
    }
}
