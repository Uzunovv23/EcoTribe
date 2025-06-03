using EcoTribe.BusinessObjects.Domain.Models;
using EcoTribe.BusinessObjects.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoTribe.Services.Interfaces
{
    public interface IAdminService
    {
        Task<List<ApplicationUser>> GetAllUsersAsync();
        Task<string> GetUserRoleAsync(ApplicationUser user);
        Task<bool> AddOrganizatorRoleAsync(string userId);
        Task<bool> RemoveOrganizatorRoleAsync(string userId);
        Task<UserManagementViewModel> GetUserManagementDataAsync();
    }
}
