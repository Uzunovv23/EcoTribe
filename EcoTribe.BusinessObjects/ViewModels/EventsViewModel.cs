using EcoTribe.BusinessObjects.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoTribe.BusinessObjects.ViewModels
{
    public class EventsViewModel
    {
        public List<EventViewModel> Events { get; set; }
        public OrganizationViewModel? UserApprovedOrganization { get; set; }
    }
}
