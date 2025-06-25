using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using EcoTribe.BusinessObjects.Domain.Models;

namespace EcoTribe.Data.Context
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>, IAppDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        { }

        public DbSet<Volunteer> Volunteers { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<EventVolunteer> EventVolunteers { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<EventResource> EventResources { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<EventSponsor> EventSponsors { get; set; }
        public DbSet<UserOrganization> UserOrganizations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Event>()
                .Property(e => e.Latitude)
                .HasPrecision(9, 6);

            modelBuilder.Entity<Event>()
                .Property(e => e.Longitude)
                .HasPrecision(9, 6);

            modelBuilder.Entity<EventSponsor>()
                .HasKey(es => new { es.EventId, es.OrganizationId });

            modelBuilder.Entity<EventSponsor>()
                .HasOne(es => es.Event)
                .WithMany(e => e.EventSponsors)
                .HasForeignKey(es => es.EventId)
                .OnDelete(DeleteBehavior.Cascade);  

            modelBuilder.Entity<EventSponsor>()
                .HasOne(es => es.Organization)
                .WithMany(o => o.EventSponsors)
                .HasForeignKey(es => es.OrganizationId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserOrganization>()
                .HasKey(uo => new { uo.UserId, uo.OrganizationId });

            modelBuilder.Entity<UserOrganization>()
                .HasOne(uo => uo.User)
                .WithMany(u => u.UserOrganizations)
                .HasForeignKey(uo => uo.UserId);

            modelBuilder.Entity<UserOrganization>()
                .HasOne(uo => uo.Organization)
                .WithMany(o => o.UserOrganizations)
                .HasForeignKey(uo => uo.OrganizationId);
        }
    }
}
