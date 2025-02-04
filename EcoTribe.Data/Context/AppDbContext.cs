using EcoTribe.BusinessObjects.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace EcoTribe.Data.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        { 
        }
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
            // Fluent API конфигурации
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
