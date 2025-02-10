using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoTribe.BusinessObjects.InputModels
{
    public class EventResourceInputModel
    {
        [Required]
        public int EventId { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Resource name cannot exceed 100 characters.")]
        public string ResourceName { get; set; } = null!;

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }

        [StringLength(100, ErrorMessage = "Provided By cannot exceed 100 characters.")]
        public string? ProvidedBy { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Status cannot exceed 50 characters.")]
        public string Status { get; set; } = null!;

        [StringLength(500, ErrorMessage = "Notes cannot exceed 500 characters.")]
        public string? Notes { get; set; }
    }
}
