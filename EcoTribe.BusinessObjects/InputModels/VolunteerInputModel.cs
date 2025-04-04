using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoTribe.BusinessObjects.InputModels
{
    public class VolunteerInputModel
    {
        [Required]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters.")]
        public string Name { get; set; } = null!;

        [Required]
        [StringLength(50, ErrorMessage = "City name cannot exceed 50 characters.")]
        public string City { get; set; } = null!;

        [Required]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; } = null!;

        [StringLength(200, ErrorMessage = "Skills description cannot exceed 200 characters.")]
        public string? Skills { get; set; }

        [StringLength(200, ErrorMessage = "Preferred events description cannot exceed 200 characters.")]
        public string? PreferredEvents { get; set; }

        [Required]
        [Phone(ErrorMessage = "Invalid phone number format.")]
        public string Number { get; set; } = null!;

        [Url(ErrorMessage = "Invalid Instagram URL format.")]
        public string? Instagram { get; set; }

        [Url(ErrorMessage = "Invalid Facebook URL format.")]
        public string? Facebook { get; set; }
    }
}
