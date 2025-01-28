using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoTribe.BusinessObjects.Models
{
    internal class EventResources
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public Event Event { get; set; } = null!;
        public string ResourceName { get; set; } = null!;
        public int Quantity { get; set; }
        public string? ProvidedBy { get; set; }
        public string Status { get; set; }
        public string? Notes { get; set; }
    }
}
