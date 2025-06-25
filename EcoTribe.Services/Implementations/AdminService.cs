using EcoTribe.BusinessObjects.Domain.Enums;
using EcoTribe.BusinessObjects.Domain.Models;
using EcoTribe.BusinessObjects.ViewModels;
using EcoTribe.Data.Context;
using EcoTribe.Services.Interfaces;
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
    public class AdminService : IAdminService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAppDbContext _context;
        private readonly IModelConverter _modelConverter;

        public AdminService(UserManager<ApplicationUser> userManager, IAppDbContext context, IModelConverter modelConverter)
        {
            _userManager = userManager;
            _context = context;
            _modelConverter = modelConverter;
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
                .Where(o => o.Status == OrganizationStatus.Pending)
                .Select(o => _modelConverter.ConvertToViewModel<Organization, OrganizationViewModel>(o))
                .ToList();

            return new UserManagementViewModel
            {
                Users = userViewModels
            };
        }

    }
}
