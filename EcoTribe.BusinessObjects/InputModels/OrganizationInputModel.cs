using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoTribe.BusinessObjects.InputModels
{
    public class OrganizationInputModel
    {
        [Required]
        public string Name { get; set; } = null!;

        [Required, EmailAddress]
        public string ContactEmail { get; set; } = null!;

        [Phone]
        public string? Phone { get; set; }

        [Url]
        public string? Website { get; set; }

        [Required]
        public string Description { get; set; } = null!;

        [Required]
        public string City { get; set; } = null!;
    }
}
