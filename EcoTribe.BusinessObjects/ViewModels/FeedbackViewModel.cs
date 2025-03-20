using EcoTribe.BusinessObjects.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoTribe.BusinessObjects.ViewModels
{
    public class FeedbackViewModel
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public Event Event { get; set; } = null!;
        public int VolunteerId { get; set; }
        public Volunteer Volunteer { get; set; } = null!;
        public int Rating { get; set; }
        public string? Comments { get; set; }
        public DateTime CreatedAt { get; set; }

        public string ApplicationUserId { get; set; } = null!;
        public string ApplicationUserName { get; set; } = null!;
        public string VolunteerName { get; set; } = string.Empty;

    }
}
