using EcoTribe.BusinessObjects.Domain.Models;
using EcoTribe.BusinessObjects.ViewModels;
using EcoTribe.Services.Implementations;
using EcoTribe.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
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

            var unapprovedOrgs = await _organizationService.GetUnapprovedOrganizationsAsync();

            var viewModel = new UserManagementViewModel
            {
                Users = userViewModels,
                UnapprovedOrganizations = unapprovedOrgs
            };

            return View(viewModel);
        }



        [HttpPost]
        public async Task<IActionResult> AddOrganizator(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            if (await _userManager.IsInRoleAsync(user, "Administrator"))
            {
                TempData["Error"] = "Administrators cannot be assigned as Organizators.";
                return RedirectToAction("Index");
            }

            var success = await _adminService.AddOrganizatorRoleAsync(userId);
            if (!success)
            {
                TempData["Error"] = "Failed to assign the Organizator role.";
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }


        [HttpPost]
        public async Task<IActionResult> RemoveOrganizator(string userId)
        {
            var success = await _adminService.RemoveOrganizatorRoleAsync(userId);
            if (!success) return NotFound();

            return RedirectToAction("Index");
        }

    }
}
