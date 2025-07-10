using EcoTribe.BusinessObjects.Domain.Models;
using EcoTribe.BusinessObjects.ViewModels;
using EcoTribe.Services.Utils;
using NUnit.Framework;

using EcoTribe.BusinessObjects.Domain.Models;
using EcoTribe.BusinessObjects.Domain.Enums;

namespace EcoTribe.UnitTests;

public class EntityToViewModelTest
{
    private IModelConverter _modelConverter;
    [SetUp]
    public void Setup()
    {
        this._modelConverter = new ModelConverter();
    }

    [Test]
    public void FeedbackEntityToViewModelTest()
    {
        var feedback = new Feedback
        {
            Id = 1,
            EventId = 100,
            VolunteerId = 200,
            Rating = 5,
            Comments = "Great event!",
            CreatedAt = new DateTime(2024, 6, 24),
            Event = new Event { Id = 100, Name = "Clean-up Day" },
        };

        var viewModel = _modelConverter.ConvertToViewModel<Feedback, FeedbackViewModel>(feedback);

        Assert.That(viewModel.Id, Is.EqualTo(feedback.Id));
        Assert.That(viewModel.EventId, Is.EqualTo(feedback.EventId));
        Assert.That(viewModel.VolunteerId, Is.EqualTo(feedback.VolunteerId));
        Assert.That(viewModel.Rating, Is.EqualTo(feedback.Rating));
        Assert.That(viewModel.Comments, Is.EqualTo(feedback.Comments));
        Assert.That(viewModel.CreatedAt, Is.EqualTo(feedback.CreatedAt));

        Assert.That(viewModel.ApplicationUserId, Is.Null.Or.Empty);
        Assert.That(viewModel.ApplicationUserName, Is.Null.Or.Empty);
        Assert.That(viewModel.VolunteerName, Is.Null.Or.Empty);
    }

    [Test]
    public void EventEntityToViewModelTest()
    {
        var now = DateTime.UtcNow;
        var ev = new Event
        {
            Id = 1,
            City = "Plovdiv",
            Name = "Eco Cleanup",
            Type = "Environmental",
            RequiredVolunteers = 15,
            CreatedBy = 5,
            Description = "A city-wide eco event",
            Latitude = 42.1354m,
            Longitude = 24.7453m,
            Start = now,
            End = now.AddHours(4)
        };

        var viewModel = _modelConverter.ConvertToViewModel<Event, EventViewModel>(ev);

        Assert.Multiple(() =>
        {
            Assert.That(viewModel.Id, Is.EqualTo(ev.Id));
            Assert.That(viewModel.City, Is.EqualTo(ev.City));
            Assert.That(viewModel.Name, Is.EqualTo(ev.Name));
            Assert.That(viewModel.Type, Is.EqualTo(ev.Type));
            Assert.That(viewModel.RequiredVolunteers, Is.EqualTo(ev.RequiredVolunteers));
            Assert.That(viewModel.CreatedBy, Is.EqualTo(ev.CreatedBy));
            Assert.That(viewModel.Description, Is.EqualTo(ev.Description));
            Assert.That(viewModel.Latitude, Is.EqualTo(ev.Latitude));
            Assert.That(viewModel.Longitude, Is.EqualTo(ev.Longitude));
            Assert.That(viewModel.Start, Is.EqualTo(ev.Start));
            Assert.That(viewModel.End, Is.EqualTo(ev.End));
        });
    }

    [Test]
    public void OrganizationEntityToViewModelTest()
    {
        var organization = new Organization
        {
            Id = 1,
            Name = "EcoCorp",
            ContactEmail = "contact@ecocorp.org",
            Phone = "123-456-7890",
            Website = "https://ecocorp.org",
            Description = "An organization for eco-friendly initiatives",
            City = "Green City",
            CreatedAt = new DateTime(2024, 10, 1),
            Status = OrganizationStatus.Approved
        };

        var viewModel = _modelConverter.ConvertToViewModel<Organization, OrganizationViewModel>(organization);

        Assert.Multiple(() =>
        {
            Assert.That(viewModel.Id, Is.EqualTo(organization.Id));
            Assert.That(viewModel.Name, Is.EqualTo(organization.Name));
            Assert.That(viewModel.ContactEmail, Is.EqualTo(organization.ContactEmail));
            Assert.That(viewModel.Phone, Is.EqualTo(organization.Phone));
            Assert.That(viewModel.Website, Is.EqualTo(organization.Website));
            Assert.That(viewModel.Description, Is.EqualTo(organization.Description));
            Assert.That(viewModel.City, Is.EqualTo(organization.City));
            Assert.That(viewModel.CreatedAt, Is.EqualTo(organization.CreatedAt));
            Assert.That(viewModel.Status, Is.EqualTo(organization.Status));
        });
    }

    [Test]
    public void NotificationEntityToViewModelTest()
    {
        var notification = new Notification
        {
            Id = 1,
            Title = "Reminder",
            Message = "Don't forget your event tomorrow!",
            CreatedAt = new DateTime(2025, 6, 25, 9, 0, 0),
            IsRead = false,
            UserId = "user123",
            EventId = 42
        };

        var viewModel = _modelConverter.ConvertToViewModel<Notification, NotificationViewModel>(notification);

        Assert.Multiple(() =>
        {
            Assert.That(viewModel.Id, Is.EqualTo(notification.Id));
            Assert.That(viewModel.Title, Is.EqualTo(notification.Title));
            Assert.That(viewModel.Message, Is.EqualTo(notification.Message));
            Assert.That(viewModel.IsRead, Is.EqualTo(notification.IsRead));
            Assert.That(viewModel.CreatedAt, Is.EqualTo(notification.CreatedAt));
        });        
    }

    [Test]
    public void VolunteerEntityToViewModelTest()
    {
        var volunteer = new Volunteer
        {
            Id = 1,
            Name = "Anna Petrova",
            City = "Varna",
            Email = "anna@volunteers.bg",
            Skills = "First Aid, Communication",
            PreferredEvents = "Environmental, Medical",
            Number = "0888123456",
            Instagram = "@anna_volunteer",
            Facebook = "facebook.com/anna.volunteer",
            DateOfBirth = new DateTime(1998, 3, 10),
            UserId = "user-123"
        };

        var viewModel = _modelConverter.ConvertToViewModel<Volunteer, VolunteerViewModel>(volunteer);

        Assert.Multiple(() =>
        {
            Assert.That(viewModel.Id, Is.EqualTo(volunteer.Id));
            Assert.That(viewModel.Name, Is.EqualTo(volunteer.Name));
            Assert.That(viewModel.City, Is.EqualTo(volunteer.City));
            Assert.That(viewModel.Email, Is.EqualTo(volunteer.Email));
            Assert.That(viewModel.Skills, Is.EqualTo(volunteer.Skills));
            Assert.That(viewModel.PreferredEvents, Is.EqualTo(volunteer.PreferredEvents));
            Assert.That(viewModel.Number, Is.EqualTo(volunteer.Number));
            Assert.That(viewModel.Instagram, Is.EqualTo(volunteer.Instagram));
            Assert.That(viewModel.Facebook, Is.EqualTo(volunteer.Facebook));
        });
    }
    [Test]
    public void EventSponsorEntityToViewModelTest()
    {
        var model = new EventSponsor
        {
            Id = 1,
            EventId = 10,
            Event = new Event { Id = 10 },
            OrganizationId = 5,
            Organization = new Organization
            {
                Id = 5,
                Name = "GreenFuture",
                Description = "Environmental NGO",
                Website = "https://greenfuture.org",
                Phone = "0898123456",
                ContactEmail = "contact@greenfuture.org",
                City = "Sofia",
                CreatedAt = DateTime.UtcNow
            },

        };

        var viewModel = _modelConverter.ConvertToViewModel<EventSponsor, EventSponsorViewModel>(model);

        Assert.Multiple(() =>
        {
            Assert.That(viewModel.Id, Is.EqualTo(model.Id));
            Assert.That(viewModel.EventId, Is.EqualTo(model.EventId));
            Assert.That(viewModel.Event.Id, Is.EqualTo(model.Event.Id));
            Assert.That(viewModel.OrganizationId, Is.EqualTo(model.OrganizationId));
            Assert.That(viewModel.Organization.Id, Is.EqualTo(model.Organization.Id));
        });
    }
    [Test]
    public void EventDetailsEntityToViewModelTest()
    {
        var eventEntity = new Event
        {
            Id = 1,
            Name = "Tree Planting",
            City = "Plovdiv",
            Type = "Environmental",
            RequiredVolunteers = 50,
            Description = "Planting trees in the park",
            Latitude = 42.1354m,
            Longitude = 24.7453m,
            Start = new DateTime(2025, 7, 10, 10, 0, 0),
            End = new DateTime(2025, 7, 10, 16, 0, 0),
            EventVolunteers = new List<EventVolunteer>(),
            EventSponsors = new List<EventSponsor>(),
            Feedbacks = new List<Feedback>()
        };

        var viewModel = _modelConverter.ConvertToViewModel<Event, EventDetailsViewModel>(eventEntity);

        Assert.Multiple(() =>
        {
            Assert.That(viewModel.Id, Is.EqualTo(eventEntity.Id));
            Assert.That(viewModel.Name, Is.EqualTo(eventEntity.Name));
            Assert.That(viewModel.City, Is.EqualTo(eventEntity.City));
            Assert.That(viewModel.Type, Is.EqualTo(eventEntity.Type));
            Assert.That(viewModel.RequiredVolunteers, Is.EqualTo(eventEntity.RequiredVolunteers));
            Assert.That(viewModel.Description, Is.EqualTo(eventEntity.Description));
            Assert.That(viewModel.Latitude, Is.EqualTo(eventEntity.Latitude));
            Assert.That(viewModel.Longitude, Is.EqualTo(eventEntity.Longitude));
            Assert.That(viewModel.Start, Is.EqualTo(eventEntity.Start));
            Assert.That(viewModel.End, Is.EqualTo(eventEntity.End));
            Assert.That(viewModel.IsParticipating, Is.False);
            Assert.That(viewModel.AttendingVolunteers, Is.Empty);
            Assert.That(viewModel.Sponsors, Is.Empty);
            Assert.That(viewModel.Feedbacks, Is.Empty);
        });
    }

    [Test]
    public void LocationEntityToViewModelTest()
    {
        var location = new Location
        {
            Id = 1,
            Name = "Nature Park",
            City = "Varna",
            Latitude = 43.2047m,
            Longitude = 27.9104m,
            Description = "A peaceful area for tree planting"
        };

        var viewModel = _modelConverter.ConvertToViewModel<Location, LocationViewModel>(location);

        Assert.Multiple(() =>
        {
            Assert.That(viewModel.Id, Is.EqualTo(location.Id));
            Assert.That(viewModel.Name, Is.EqualTo(location.Name));
            Assert.That(viewModel.City, Is.EqualTo(location.City));
            Assert.That(viewModel.Latitude, Is.EqualTo(location.Latitude));
            Assert.That(viewModel.Longitude, Is.EqualTo(location.Longitude));
            Assert.That(viewModel.Description, Is.EqualTo(location.Description));
        });
    }

    [Test]
    public void ApplicationUserToRegisterViewModelTest()
    {
        var user = new ApplicationUser
        {
            Email = "user@example.com",
            FirstName = "Ivan",
            LastName = "Petrov"
        };

        var viewModel = _modelConverter.ConvertToViewModel<ApplicationUser, RegisterViewModel>(user);

        Assert.Multiple(() =>
        {
            Assert.That(viewModel.Email, Is.EqualTo(user.Email));
            Assert.That(viewModel.FirstName, Is.EqualTo(user.FirstName));
            Assert.That(viewModel.LastName, Is.EqualTo(user.LastName));
            Assert.That(viewModel.Password, Is.Null.Or.Empty);
            Assert.That(viewModel.ConfirmPassword, Is.Null.Or.Empty);
        });
    }

    [Test]
    public void ApplicationUserToLoginViewModelTest()
    {
        var user = new ApplicationUser
        {
            Email = "user@example.com"
        };

        var viewModel = _modelConverter.ConvertToViewModel<ApplicationUser, LoginViewModel>(user);

        Assert.Multiple(() =>
        {
            Assert.That(viewModel.Email, Is.EqualTo(user.Email));
            Assert.That(viewModel.Password, Is.Null.Or.Empty); 
            Assert.That(viewModel.RememberMe, Is.False);        
        });
    }
}
