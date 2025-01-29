using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoTribe.BusinessObjects.Models
{
    public class Event
    {
        public int Id { get; set; }
        public string City { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Type { get; set; } = null!;
        public int RequiredVolunteers { get; set; }
        public int CreatedBy { get; set; } 
        public string Description { get; set; } = null!;
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}
