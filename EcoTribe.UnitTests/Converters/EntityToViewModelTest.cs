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

}
