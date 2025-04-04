using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoTribe.BusinessObjects.Domain.Models
{
    public class Volunteer
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Skills { get; set; }
        public string? PreferredEvents { get; set; }
        public string Number { get; set; } = null!;
        public string? Instagram { get; set; }
        public string? Facebook { get; set; }
        public DateTime DateOfBirth { get; set; }
        public ICollection<EventVolunteer>? EventVolunteers { get; set; }
        public ICollection<Feedback>? Feedbacks { get; set; }


        public string? UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual ApplicationUser User { get; set; } = null!;
    }
}
