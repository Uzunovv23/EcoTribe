﻿using Microsoft.AspNetCore.Identity;

namespace EcoTribe.BusinessObjects.Domain.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; } 
        public string? Address { get; set; }

        public ICollection<UserOrganization> UserOrganizations { get; set; } = new List<UserOrganization>();
    }
}
