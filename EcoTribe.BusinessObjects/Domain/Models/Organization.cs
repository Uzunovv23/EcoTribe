using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoTribe.BusinessObjects.Domain.Models
{
    public class Organization
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string ContactEmail { get; set; } = null!;
        public string? Phone { get; set; }
        public string? Website { get; set; }
        public string Description { get; set; } = null!;
        public string City { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public bool Approved { get; set; } = false;

        public ICollection<EventSponsor> EventSponsors { get; set; } = new List<EventSponsor>();

        public ICollection<UserOrganization> UserOrganizations { get; set; } = new List<UserOrganization>();

    }
}
