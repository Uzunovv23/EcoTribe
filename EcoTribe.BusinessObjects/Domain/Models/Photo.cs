using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoTribe.BusinessObjects.Domain.Models
{
    public class Photo
    {
        public int Id { get; set; }
        public string Url { get; set; } = null!;
        public DateTime UploadedOn { get; set; }

        public ICollection<EventPhoto> EventPhotos { get; set; } = new List<EventPhoto>();
        public ICollection<VolunteerPhoto> VolunteerPhotos { get; set; } = new List<VolunteerPhoto>();
        public ICollection<OrganizationPhoto> OrganizationPhotos { get; set; } = new List<OrganizationPhoto>();
    }
}
