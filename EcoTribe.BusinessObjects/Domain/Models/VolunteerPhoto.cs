using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoTribe.BusinessObjects.Domain.Models
{
    public class VolunteerPhoto
    {
        public int Id { get; set; }

        public int VolunteerId { get; set; }
        public Volunteer Volunteer { get; set; } = null!;

        public int PhotoId { get; set; }
        public Photo Photo { get; set; } = null!;
    }
}
