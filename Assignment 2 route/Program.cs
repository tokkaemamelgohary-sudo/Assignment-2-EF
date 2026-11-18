using EventHub.Data;
using EventHub.Models;
using Microsoft.EntityFrameworkCore;

using var context = new EventHubDbContext();

var organizer = new Organizer
{
    Name = "Ahmed Ali",
    CompanyName = "Route Events",
    IsVerified = true,
    Profile = new OrganizerProfile
    {
        Biography = "We organize tech events.",
        WebsiteUrl = "https://example.com",
        LogoUrl = "https://example.com/logo.png"
    }
};

organizer.Events.Add(new Event
{
    Title = "EF Core Workshop",
    Description = "A hands-on workshop about EF Core relationships.",
    StartDate = new DateTime(2026, 12, 1, 10, 0, 0),
    EndDate = new DateTime(2026, 12, 1, 16, 0, 0),
    MaxAttendees = 100
});

var attendee = new Attendee
{
    FullName = "Sara Mohamed",
    Email = "sara@example.com",
    Street = "10 Pyramids St",
    City = "Giza",
    Country = "Egypt",
    PostalCode = "12511",
    Badge = new Badge
    {
        BadgeNumber = 1001,
        IssuedDate = DateTime.Today,
        Tier = BadgeTier.VIP
    }
};
if (!context.Attendees.Any(a => a.Email == attendee.Email))
{
    context.Organizers.Add(organizer);
    context.Attendees.Add(attendee);
    context.SaveChanges();
}
else
{
    Console.WriteLine("Data already exists, skipping insert.");
}

var org = context.Organizers
    .Include(o => o.Profile)
    .Include(o => o.Events)
    .First();

Console.WriteLine($"Organizer: {org.Name} ({org.CompanyName}) - Verified: {org.IsVerified}");
Console.WriteLine($"Bio: {org.Profile?.Biography}");
foreach (var ev in org.Events)
    Console.WriteLine($"Event: {ev.Title} | {ev.StartDate:d} | Max: {ev.MaxAttendees}");

var att = context.Attendees.Include(a => a.Badge).First();
Console.WriteLine($"Attendee: {att.FullName} | {att.FullAddress}");
Console.WriteLine($"Badge #{att.Badge?.BadgeNumber} - {att.Badge?.Tier} - {att.Badge?.IssuedDate:d}");