using EcoTribe.BusinessObjects.Domain.Enums;
using EcoTribe.BusinessObjects.Domain.Models;
using EcoTribe.BusinessObjects.ViewModels;
using EcoTribe.Services.Implementations;
using EcoTribe.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcoTribe.Web.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IOrganizationService _organizationService;

        public AdminController(IAdminService adminService, UserManager<ApplicationUser> userManager, IOrganizationService organizationService)
        {
            _adminService = adminService;
            _userManager = userManager;
            _organizationService = organizationService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> UserManagement()
        {
            var users = await _adminService.GetAllUsersAsync();
            var userViewModels = new List<UserViewModel>();

            foreach (var user in users)
            {
                userViewModels.Add(new UserViewModel
                {
                    Id = user.Id,
                    Email = user.Email,
                    Role = await _adminService.GetUserRoleAsync(user)
                });
            }

            var viewModel = new UserManagementViewModel
            {
                Users = userViewModels
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddOrganizator(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound();

            if (await _userManager.IsInRoleAsync(user, "Administrator"))
            {
                TempData["Error"] = "Administrators cannot be assigned as Organizators.";
                return RedirectToAction("UserManagement");
            }

            var success = await _adminService.AddOrganizatorRoleAsync(userId);
            if (!success)
                TempData["Error"] = "Failed to assign the Organizator role.";

            return RedirectToAction("UserManagement");
        }

        [HttpPost]
        public async Task<IActionResult> RemoveOrganizator(string userId)
        {
            var success = await _adminService.RemoveOrganizatorRoleAsync(userId);
            if (!success)
                TempData["Error"] = "Failed to remove Organizator role.";

            return RedirectToAction("UserManagement");
        }

        public async Task<IActionResult> OrganizationManagement()
        {
            var allOrganizations = await _organizationService.GetAllOrganizationsAsync();

            var viewModel = new OrganizationManagementViewModel
            {
                AllOrganizations = allOrganizations
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> ApproveOrganization(int organizationId)
        {
            var success = await _organizationService.ApproveOrganizationAsync(organizationId);

            if (!success)
                TempData["Error"] = "Failed to approve organization.";

            return RedirectToAction("OrganizationManagement");
        }


        [HttpPost]
        public async Task<IActionResult> DisapproveOrganization(int organizationId)
        {
            var success = await _organizationService.ChangeStatusAsync(organizationId, OrganizationStatus.Disapproved);

            if (!success)
                TempData["Error"] = "Failed to disapprove organization.";

            return RedirectToAction("OrganizationManagement");
        }

        [HttpPost]
        public async Task<IActionResult> ChangeOrganizationStatus(int organizationId, OrganizationStatus newStatus)
        {
            var success = await _organizationService.ChangeStatusAsync(organizationId, newStatus);
            if (!success)
            {
                TempData["Error"] = "Failed to change organization status.";
            }

            return RedirectToAction("OrganizationManagement");
        }

        [HttpPost]
        public async Task<IActionResult> ToggleOrganizationStatus(int organizationId)
        {
            var organization = await _organizationService.GetOrganizationByIdAsync(organizationId);

            if (organization == null)
            {
                TempData["Error"] = "Organization not found.";
                return RedirectToAction("OrganizationManagement");
            }

            OrganizationStatus newStatus;

            if (organization.Status == OrganizationStatus.Approved)
                newStatus = OrganizationStatus.Disapproved;
            else if (organization.Status == OrganizationStatus.Disapproved)
                newStatus = OrganizationStatus.Approved;
            else
            {
                TempData["Error"] = "Cannot change status of a pending organization.";
                return RedirectToAction("OrganizationManagement");
            }

            var success = await _organizationService.ChangeStatusAsync(organizationId, newStatus);

            if (!success)
                TempData["Error"] = "Failed to change organization status.";

            return RedirectToAction("OrganizationManagement");
        }


    }
}
