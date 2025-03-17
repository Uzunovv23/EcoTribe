using EcoTribe.BusinessObjects.Domain.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoTribe.Services.Implementations
{
    public class AdminService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<List<ApplicationUser>> GetAllUsersAsync()
        {
            return _userManager.Users.ToList();
        }

        public async Task<string> GetUserRoleAsync(ApplicationUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            return roles.FirstOrDefault() ?? "User";
        }

        public async Task<bool> AddOrganizatorRoleAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            await _userManager.RemoveFromRoleAsync(user, "User");
            var result = await _userManager.AddToRoleAsync(user, "Organizator");

            return result.Succeeded;
        }

        public async Task<bool> RemoveOrganizatorRoleAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            await _userManager.RemoveFromRoleAsync(user, "Organizator");
            var result = await _userManager.AddToRoleAsync(user, "User");

            return result.Succeeded;
        }
    }
}
