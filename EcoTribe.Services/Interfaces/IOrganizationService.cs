using EcoTribe.BusinessObjects.InputModels;
using EcoTribe.BusinessObjects.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoTribe.Services.Interfaces
{
    public interface IOrganizationService : IService<OrganizationViewModel, OrganizationInputModel>
    {
        public OrganizationViewModel? GetByUserId(string userId);
        public Task CreateAsync(OrganizationInputModel inputModel, string userId);
        public Task<List<OrganizationViewModel>> GetUnapprovedOrganizationsAsync();
    }
}
