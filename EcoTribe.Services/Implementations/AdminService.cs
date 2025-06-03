using EcoTribe.BusinessObjects.Domain.Models;
using EcoTribe.BusinessObjects.ViewModels;
using EcoTribe.Data.Context;
using EcoTribe.Services.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
        private readonly AppDbContext _context;

        public AdminService(UserManager<ApplicationUser> userManager, AppDbContext context)
        {
            _userManager = userManager;
            _context = context;
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

        public async Task<UserManagementViewModel> GetUserManagementDataAsync()
        {
            var users = await _userManager.Users.ToListAsync();

            var userViewModels = new List<UserViewModel>();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userViewModels.Add(new UserViewModel
                {
                    Id = user.Id,
                    Email = user.Email!,
                    Name = user.UserName!,
                    Role = roles.FirstOrDefault() ?? "User"
                });
            }

            var unapprovedOrganizations = _context.Organizations
                .Where(o => !o.Approved)
                .Select(o => ModelConverter.ConvertToViewModel<Organization, OrganizationViewModel>(o))
                .ToList();

            return new UserManagementViewModel
            {
                Users = userViewModels,
                UnapprovedOrganizations = unapprovedOrganizations
            };
        }
    }
}
