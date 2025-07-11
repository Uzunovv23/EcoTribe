﻿using EcoTribe.BusinessObjects.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoTribe.BusinessObjects.ViewModels
{
    public class TestViewModel
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public Event Event { get; set; } = null!;
        public int VolunteerId { get; set; }
        public Volunteer Volunteer { get; set; } = null!;

        [MapFrom("Rating")]
        public int Reiting { get; set; }

        [MapFrom("Comments")]
        public string? Komentari { get; set; }
        public DateTime CreatedAt { get; set; }

        public string ApplicationUserId { get; set; } = null!;
        public string ApplicationUserName { get; set; } = null!;
        public string? VolunteerName { get; set; }
    }
}
