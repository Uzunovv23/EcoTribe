using EcoTribe.BusinessObjects.Domain.Models;
using EcoTribe.Services.Utils;
using EcoTribe.BusinessObjects.ViewModels;
using EcoTribe.Data.Context;
using EcoTribe.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using EcoTribe.BusinessObjects.InputModels;
using Microsoft.EntityFrameworkCore;

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
            organization.CreatedAt = DateTime.UtcNow; 
            context.Organizations.Add(organization);
            context.SaveChanges();
        }

        public async Task CreateAsync(OrganizationInputModel inputModel, string userId)
        {

            var organization = ModelConverter.ConvertToModel<OrganizationInputModel, Organization>(inputModel);

            organization.CreatedAt = DateTime.UtcNow;
            organization.Approved = false;

            context.Organizations.Add(organization);
            await context.SaveChangesAsync(); 

            var userOrganization = new UserOrganization
            {
                UserId = userId,
                OrganizationId = organization.Id
            };

            context.UserOrganizations.Add(userOrganization);
            await context.SaveChangesAsync();
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
                .Where(o => o.UserOrganizations.Any(uo => uo.UserId == userId) && o.Approved)
                .FirstOrDefault();

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

        public async Task<List<OrganizationViewModel>> GetUnapprovedOrganizationsAsync()
        {
            return await context.Organizations
                .Where(o => !o.Approved)
                .Select(o => new OrganizationViewModel
                {
                    Id = o.Id,
                    Name = o.Name,
                    ContactEmail = o.ContactEmail
                })
                .ToListAsync();
        }

        public async Task<bool> ApproveOrganizationAsync(int organizationId)
        {
            var organization = await context.Organizations
                .FirstOrDefaultAsync(o => o.Id == organizationId);

            if (organization == null)
                return false;

            organization.Approved = true;

            context.Organizations.Update(organization);
            await context.SaveChangesAsync();

            return true;
        }
    }
}
