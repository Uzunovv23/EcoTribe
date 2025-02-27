using Microsoft.AspNetCore.Identity;

namespace EcoTribe.BusinessObjects.Domain.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Address { get; set; } = null!;
        public DateTime DateOfBirth { get; set; }
        public ICollection<EventVolunteer>? EventVolunteers { get; set; }
        public ICollection<Feedback>? Feedbacks { get; set; }
    }
}
