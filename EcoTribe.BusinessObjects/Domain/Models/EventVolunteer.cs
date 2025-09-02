using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoTribe.BusinessObjects.Domain.Models
{
    public class EventVolunteer
    {
        public int Id { get; set; }

        [ForeignKey(nameof(Volunteer))]
        public int VolunteerId { get; set; }
        public Volunteer Volunteer { get; set; } = null!;

        [ForeignKey(nameof(Event))]
        public int EventId { get; set; }
        public Event Event { get; set; } = null!;

        public string Intention { get; set; } = null!;
        public bool? Attended { get; set; }
        public int Points { get; set; }
        public ICollection<EventPhoto> EventPhotos { get; set; } = new List<EventPhoto>();
    }
}
