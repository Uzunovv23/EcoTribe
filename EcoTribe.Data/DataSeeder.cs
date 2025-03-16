using EcoTribe.BusinessObjects.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoTribe.Data
{
    public class DataSeeder
    {
        public static async Task SeedRolesAndAdmin(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            string[] roleNames = { "Administrator", "User", "Organizator" };

            foreach (var role in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            string adminEmail = "admin@example.com";
            string adminPassword = "Admin123!";

            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                var newAdmin = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true,
                    FirstName = "Admin" 
                };

                var result = await userManager.CreateAsync(newAdmin, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(newAdmin, "Administrator");
                }
            }

            string organizatorEmail = "organizator@example.com";
            string organizatorPassoword = "Organizator123!";

            var organizatorUser = await userManager.FindByEmailAsync(organizatorEmail);
            if (organizatorUser == null)
            {
                var newOrganizator = new ApplicationUser
                {
                    UserName = organizatorEmail,
                    Email = organizatorEmail,
                    EmailConfirmed = true,
                    FirstName = "Organizator"
                };

                var organizatorResult = await userManager.CreateAsync(newOrganizator, organizatorPassoword);
                if (organizatorResult.Succeeded)
                {
                    await userManager.AddToRoleAsync(newOrganizator, "Organizator");
                }
            }
        }
    }
}
