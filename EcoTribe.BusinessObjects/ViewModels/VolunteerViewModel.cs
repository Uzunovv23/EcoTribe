﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoTribe.BusinessObjects.ViewModels
{
    public class VolunteerViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Skills { get; set; }
        public string? PreferredEvents { get; set; }
        public string Number { get; set; } = null!;
        public string? Instagram { get; set; }
        public string? Facebook { get; set; }
    }
}
