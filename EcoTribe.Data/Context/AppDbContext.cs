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
        public DbSet<Photo> Photos { get; set; }
        public DbSet<EventPhoto> EventPhotos { get; set; }
        public DbSet<VolunteerPhoto> VolunteerPhotos { get; set; }
        public DbSet<OrganizationPhoto> OrganizationPhotos { get; set; }

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

            modelBuilder.Entity<EventPhoto>()
                .HasKey(ep => new { ep.EventId, ep.PhotoId });

            modelBuilder.Entity<EventPhoto>()
                .HasOne(ep => ep.Event)
                .WithMany(e => e.EventPhotos)
                .HasForeignKey(ep => ep.EventId);

            modelBuilder.Entity<EventPhoto>()
                .HasOne(ep => ep.Photo)
                .WithMany(p => p.EventPhotos)
                .HasForeignKey(ep => ep.PhotoId);

            modelBuilder.Entity<VolunteerPhoto>()
                .HasKey(vp => new { vp.VolunteerId, vp.PhotoId });

            modelBuilder.Entity<VolunteerPhoto>()
                .HasOne(vp => vp.Volunteer)
                .WithMany(v => v.VolunteerPhotos)
                .HasForeignKey(vp => vp.VolunteerId);

            modelBuilder.Entity<VolunteerPhoto>()
                .HasOne(vp => vp.Photo)
                .WithMany(p => p.VolunteerPhotos)
                .HasForeignKey(vp => vp.PhotoId);

            modelBuilder.Entity<OrganizationPhoto>()
                .HasKey(op => new { op.OrganizationId, op.PhotoId });

            modelBuilder.Entity<OrganizationPhoto>()
                .HasOne(op => op.Organization)
                .WithMany(o => o.OrganizationPhotos)
                .HasForeignKey(op => op.OrganizationId);

            modelBuilder.Entity<OrganizationPhoto>()
                .HasOne(op => op.Photo)
                .WithMany(p => p.OrganizationPhotos)
                .HasForeignKey(op => op.PhotoId);
        }
    }
}
