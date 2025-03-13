using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoTribe.BusinessObjects.InputModels
{
    public class EventSponsorInputModel
    {
        [Required]
        public int EventId { get; set; }

        [Required]
        public int OrganizationId { get; set; }
    }
}

