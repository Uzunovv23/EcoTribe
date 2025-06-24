using EcoTribe.BusinessObjects.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoTribe.Services.Utils
{
    public class UserManagerService : IUserManagerService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserManagerService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<List<ApplicationUser>> GetAllUsersAsync()
        {
            return await _userManager.Users.ToListAsync();
        }

        public async Task<string> GetUserRoleAsync(ApplicationUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            return roles.FirstOrDefault() ?? "User";
        }

        public async Task<bool> AddRoleAsync(ApplicationUser user, string role)
        {
            var result = await _userManager.AddToRoleAsync(user, role);
            return result.Succeeded;
        }

        public async Task<bool> RemoveRoleAsync(ApplicationUser user, string role)
        {
            var result = await _userManager.RemoveFromRoleAsync(user, role);
            return result.Succeeded;
        }

        public Task<ApplicationUser?> FindByIdAsync(string userId)
            => _userManager.FindByIdAsync(userId);

        public Task<IList<string>> GetRolesAsync(ApplicationUser user)
            => _userManager.GetRolesAsync(user);
    }
}
