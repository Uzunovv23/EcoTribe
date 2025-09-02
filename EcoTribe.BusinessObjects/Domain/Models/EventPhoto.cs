using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcoTribe.BusinessObjects.Domain.Models
{
    public class EventPhoto
    {
        public int Id { get; set; }

        [Required]
        [ForeignKey(nameof(Event))]
        public int EventId { get; set; }
        public Event Event { get; set; } = null!;

        [ForeignKey(nameof(EventVolunteer))]
        public int? EventVolunteerId { get; set; }
        public EventVolunteer? EventVolunteer { get; set; }

        [Required]
        [MaxLength(255)]
        public string FilePath { get; set; } = null!; 

        [MaxLength(500)]
        public string? Description { get; set; }

        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
    }
}
