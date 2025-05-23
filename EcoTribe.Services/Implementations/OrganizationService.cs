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

        public OrganizationViewModel? GetById(int id)
        {
            var organization = context.Organizations.Find(id);
            return organization != null
                ? ModelConverter.ConvertToViewModel<Organization, OrganizationViewModel>(organization)
                : null;
        }

        public OrganizationViewModel? GetByUserId(string userId)
        {
            var organization = context.Organizations
                .FirstOrDefault(org => org.UserId == userId);
            return organization != null
                ? ModelConverter.ConvertToViewModel<Organization, OrganizationViewModel>(organization)
                : null;
        }

        public void Update(int id, OrganizationInputModel inputModel)
        {
            var existingOrganization = context.Organizations.Find(id);
            if (existingOrganization == null)
            {
                throw new ArgumentException("Organization not found.");
            }

            var updatedOrganization = ModelConverter.ConvertToModel<OrganizationInputModel, Organization>(inputModel);
            updatedOrganization.Id = id; 

            context.Entry(existingOrganization).CurrentValues.SetValues(updatedOrganization);

            context.SaveChanges();
        }
        public void Delete(int id)
        {
            var organization = context.Organizations.Find(id);
            if (organization == null)
            {
                throw new ArgumentException("Organization not found.");
            }

            context.Organizations.Remove(organization);
            context.SaveChanges();
        }
    }
}
