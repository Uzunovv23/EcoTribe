using EcoTribe.BusinessObjects.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoTribe.Data.Context
{
    public interface IAppDbContext
    {
        DbSet<Volunteer> Volunteers { get; set; }
        DbSet<Event> Events { get; set; }
        DbSet<EventVolunteer> EventVolunteers { get; set; }
        DbSet<Organization> Organizations { get; set; }
        DbSet<EventResource> EventResources { get; set; }
        DbSet<Feedback> Feedbacks { get; set; }
        DbSet<Location> Locations { get; set; }
        DbSet<Notification> Notifications { get; set; }
        DbSet<EventSponsor> EventSponsors { get; set; }
        DbSet<UserOrganization> UserOrganizations { get; set; }
        DbSet<ApplicationUser> Users { get; }
        EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        int SaveChanges();
    }
}
