using EcoTribe.BusinessObjects.Domain.Models;
using EcoTribe.BusinessObjects.ViewModels;
using EcoTribe.Services.Implementations;
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
        private readonly AdminService _adminService;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminController(AdminService adminService, UserManager<ApplicationUser> userManager)
        {
            _adminService = adminService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _adminService.GetAllUsersAsync();
            var userRoles = new Dictionary<string, string>();
            var admins = new List<ApplicationUser>();
            var organizators = new List<ApplicationUser>();
            var normalUsers = new List<ApplicationUser>();

            foreach (var user in users)
            {
                var role = await _adminService.GetUserRoleAsync(user);
                userRoles[user.Id] = role;

                if (role == "Administrator")
                {
                    admins.Add(user);
                }
                else if (role == "Organizator")
                {
                    organizators.Add(user);
                }
                else
                {
                    normalUsers.Add(user);
                }
            }

            var model = new AdminViewModel
            {
                Users = admins.Concat(organizators).Concat(normalUsers).ToList(),
                UserRoles = userRoles
            };

            return View(model);
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
