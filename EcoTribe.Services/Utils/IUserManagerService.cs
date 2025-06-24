using EcoTribe.BusinessObjects.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoTribe.Services.Utils
{
    public interface IUserManagerService
    {
        Task<List<ApplicationUser>> GetAllUsersAsync();
        Task<string> GetUserRoleAsync(ApplicationUser user);
        Task<bool> AddRoleAsync(ApplicationUser user, string role);
        Task<bool> RemoveRoleAsync(ApplicationUser user, string role);
        Task<ApplicationUser?> FindByIdAsync(string userId);
        Task<IList<string>> GetRolesAsync(ApplicationUser user);
    }
}
