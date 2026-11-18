using EventHub.Configurations;
using EventHub.Models;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Data;

public class EventHubDbContext : DbContext
{
    public DbSet<Organizer> Organizers => Set<Organizer>();
    public DbSet<OrganizerProfile> OrganizerProfiles => Set<OrganizerProfile>();
    public DbSet<Event> Events => Set<Event>();
    public DbSet<Attendee> Attendees => Set<Attendee>();
    public DbSet<Badge> Badges => Set<Badge>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
            "Server=(localdb)\\MSSQLLocalDB;Database=EventHubDb;Trusted_Connection=True;TrustServerCertificate=True;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<OrganizerProfile>(entity =>
        {
            entity.ToTable("OrganizerProfiles");

            entity.HasKey(p => p.Id);

            entity.Property(p => p.Biography).HasMaxLength(1000);
            entity.Property(p => p.WebsiteUrl).HasMaxLength(300);
            entity.Property(p => p.LogoUrl).HasMaxLength(500);

            entity.HasOne(p => p.Organizer)
                  .WithOne(o => o.Profile)
                  .HasForeignKey<OrganizerProfile>(p => p.OrganizerId)
                  .IsRequired()
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.ApplyConfiguration(new EventConfiguration());
        modelBuilder.ApplyConfiguration(new BadgeConfiguration());
    }
}