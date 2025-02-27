using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using EcoTribe.BusinessObjects.Domain.Models;

namespace EcoTribe.Data.Context
{
    public class AppDbContext :IdentityDbContext<ApplicationUser>
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            base.OnModelCreating(modelBuilder);

            
            modelBuilder.Entity<Volunteer>()
                .Property(v => v.Latitude)
                .HasPrecision(9, 6);

            modelBuilder.Entity<Volunteer>()
                .Property(v => v.Longitude)
                .HasPrecision(9, 6);

            modelBuilder.Entity<Event>()
                .Property(e => e.Latitude)
                .HasPrecision(9, 6);

            modelBuilder.Entity<Event>()
                .Property(e => e.Longitude)
                .HasPrecision(9, 6);
        }
    }
}
