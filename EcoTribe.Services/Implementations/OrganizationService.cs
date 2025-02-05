using EcoTribe.BusinessObjects.ViewModels;
using EcoTribe.Data.Context;
using EcoTribe.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            return context.Organizations.Select(organization => new OrganizationViewModel()
            {
                Id = organization.Id,
                Name = organization.Name,
                City = organization.City,
                Description = organization.Description,
                ContactEmail = organization.ContactEmail,
                Phone = organization.Phone,
                Website = organization.Website,
                CreatedAt = organization.CreatedAt,
            });
        }
    }
}
