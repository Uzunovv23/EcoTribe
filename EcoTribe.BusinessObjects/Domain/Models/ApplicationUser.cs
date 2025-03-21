using Microsoft.AspNetCore.Identity;

namespace EcoTribe.BusinessObjects.Domain.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; } 
        public string? Address { get; set; } 
        public DateTime DateOfBirth { get; set; }
        public ICollection<EventVolunteer>? EventVolunteers { get; set; }
        public ICollection<Feedback>? Feedbacks { get; set; }
        public int? VolunteerId { get; set; }
    }
}
