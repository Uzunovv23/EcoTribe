using EcoTribe.BusinessObjects.Domain.Models;
using EcoTribe.Services.Utils;
using EcoTribe.BusinessObjects.ViewModels;
using EcoTribe.Data.Context;
using EcoTribe.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using EcoTribe.BusinessObjects.InputModels;

namespace EcoTribe.Services.Implementations
{
    public class OrganizationService : IOrganizationService
    {
        private readonly AppDbContext context;

        public OrganizationService(AppDbContext context)
        {
            this.context = context;
        }

        public IEnumerable<OrganizationViewModel> GetAll()
        {
            return context.Organizations
                .Select(org => ModelConverter.ConvertToViewModel<Organization, OrganizationViewModel>(org))
                .ToList();
        }

        public void Create(OrganizationInputModel inputModel)
        {
            var organization = ModelConverter.ConvertToModel<OrganizationInputModel, Organization>(inputModel);
            organization.CreatedAt = DateTime.UtcNow; // Set creation date
            context.Organizations.Add(organization);
            context.SaveChanges();
        }
    }
}
