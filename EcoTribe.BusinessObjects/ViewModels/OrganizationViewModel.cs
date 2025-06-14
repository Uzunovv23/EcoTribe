﻿using EcoTribe.BusinessObjects.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoTribe.BusinessObjects.ViewModels
{
    public class OrganizationViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string ContactEmail { get; set; } = null!;
        public string? Phone { get; set; }
        public string? Website { get; set; }
        public string Description { get; set; } = null!;
        public string City { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public OrganizationStatus Status { get; set; }
    }
}
