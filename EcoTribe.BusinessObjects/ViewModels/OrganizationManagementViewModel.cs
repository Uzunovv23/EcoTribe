using EcoTribe.BusinessObjects.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoTribe.BusinessObjects.ViewModels
{
    public class OrganizationManagementViewModel
    {
        public List<OrganizationViewModel> UnapprovedOrganizations { get; set; } = new();
    }
}
