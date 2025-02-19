using EcoTribe.BusinessObjects.InputModels;
using EcoTribe.BusinessObjects.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoTribe.Services.Interfaces
{
    public interface IOrganizationService
    {
        IEnumerable<OrganizationViewModel> GetAll();
        OrganizationViewModel? GetById(int id); 
        void Create(OrganizationInputModel inputModel);
        void Update(int id, OrganizationInputModel inputModel);
    }
}
