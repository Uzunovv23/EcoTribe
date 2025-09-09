using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcoTribe.BusinessObjects.Domain.Models
{
    public class EventPhoto
    {
        public int Id { get; set; }

        public int EventId { get; set; }
        public Event Event { get; set; } = null!;

        public int PhotoId { get; set; }
        public Photo Photo { get; set; } = null!;
    }
}
