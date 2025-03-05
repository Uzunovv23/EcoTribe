using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoTribe.BusinessObjects.InputModels
{
    public class EventVolunteerInputModel
    {
        [Required]
        public int VolunteerId { get; set; }

        [Required]
        public int EventId { get; set; }

        [Required]
        [MaxLength(500)]
        public string Intention { get; set; } = null!;

        public bool? Attended { get; set; }

    }
}
