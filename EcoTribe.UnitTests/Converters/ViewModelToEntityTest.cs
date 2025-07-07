using EcoTribe.BusinessObjects.Domain.Enums;
using EcoTribe.BusinessObjects.Domain.Models;
using EcoTribe.BusinessObjects.ViewModels;
using EcoTribe.Services.Utils;
using NUnit.Framework;

namespace EcoTribe.UnitTests;

public class ViewModelToEntityTest
{
    private IModelConverter _modelConverter;
    [SetUp]
    public void Setup()
    {
        this._modelConverter = new ModelConverter();
    }

    [Test]
    public void EventViewModelToEntityTest()
    {
        var eventViewModel = new EventViewModel
        {
            Id = 1,
            City = "Sofia",
            Name = "Clean Park",
            Type = "Environmental",
            RequiredVolunteers = 10,
            CreatedBy = 42,
            Description = "Park cleanup",
            Latitude = 42.6977m,
            Longitude = 23.3219m,
            Start = new DateTime(2025, 7, 1),
            End = new DateTime(2025, 7, 2)
        };

        var result = _modelConverter.ConvertToModel<EventViewModel, Event>(eventViewModel);

        Assert.That(result.Id, Is.EqualTo(1));
        Assert.That(result.City, Is.EqualTo("Sofia"));
        Assert.That(result.Name, Is.EqualTo("Clean Park"));
        Assert.That(result.Type, Is.EqualTo("Environmental"));
        Assert.That(result.RequiredVolunteers, Is.EqualTo(10));
        Assert.That(result.CreatedBy, Is.EqualTo(42));
        Assert.That(result.Description, Is.EqualTo("Park cleanup"));
        Assert.That(result.Latitude, Is.EqualTo(42.6977m));
        Assert.That(result.Longitude, Is.EqualTo(23.3219m));
        Assert.That(result.Start, Is.EqualTo(new DateTime(2025, 7, 1)));
        Assert.That(result.End, Is.EqualTo(new DateTime(2025, 7, 2)));
    }

    [Test]
    public void FeedbackViewModelToEntityTest()
    {
        var feedbackViewModel = new FeedbackViewModel
        {
            Id = 5,
            EventId = 3,
            VolunteerId = 2,
            Rating = 4,
            Comments = "Great event!",
            CreatedAt = DateTime.UtcNow
        };

        var result = _modelConverter.ConvertToModel<FeedbackViewModel, Feedback>(feedbackViewModel);

        Assert.That(result.Id, Is.EqualTo(5));
        Assert.That(result.EventId, Is.EqualTo(3));
        Assert.That(result.VolunteerId, Is.EqualTo(2));
        Assert.That(result.Rating, Is.EqualTo(4));
        Assert.That(result.Comments, Is.EqualTo("Great event!"));
        Assert.That(result.CreatedAt, Is.EqualTo(feedbackViewModel.CreatedAt));
    }

    [Test]
    public void OrganizationViewModelToEntityTest()
    {
        var organizationViewModel = new OrganizationViewModel
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

        var result = _modelConverter.ConvertToModel<OrganizationViewModel, Organization>(organizationViewModel);

        Assert.Multiple(() =>
        {
            Assert.That(result.Id, Is.EqualTo(organizationViewModel.Id));
            Assert.That(result.Name, Is.EqualTo(organizationViewModel.Name));
            Assert.That(result.ContactEmail, Is.EqualTo(organizationViewModel.ContactEmail));
            Assert.That(result.Phone, Is.EqualTo(organizationViewModel.Phone));
            Assert.That(result.Website, Is.EqualTo(organizationViewModel.Website));
            Assert.That(result.Description, Is.EqualTo(organizationViewModel.Description));
            Assert.That(result.City, Is.EqualTo(organizationViewModel.City));
            Assert.That(result.CreatedAt, Is.EqualTo(organizationViewModel.CreatedAt));
            Assert.That(result.Status, Is.EqualTo(organizationViewModel.Status));
        });
    }

    [Test]
    public void NotificationViewModelToEntityTest()
    {
        var viewModel = new NotificationViewModel
        {
            Id = 1,
            Title = "Reminder",
            Message = "Don't forget your event tomorrow!",
            CreatedAt = new DateTime(2025, 6, 25, 9, 0, 0),
            IsRead = true
        };

        var entity = _modelConverter.ConvertToModel<NotificationViewModel, Notification>(viewModel);

        Assert.Multiple(() =>
        {
            Assert.That(entity.Id, Is.EqualTo(viewModel.Id));
            Assert.That(entity.Title, Is.EqualTo(viewModel.Title));
            Assert.That(entity.Message, Is.EqualTo(viewModel.Message));
            Assert.That(entity.CreatedAt, Is.EqualTo(viewModel.CreatedAt));
            Assert.That(entity.IsRead, Is.EqualTo(viewModel.IsRead));
        });
    }

    [Test]
    public void ConvertToModel_VolunteerViewModel_MapsPropertiesCorrectly()
    {
        var viewModel = new VolunteerViewModel
        {
            Id = 2,
            Name = "Georgi Ivanov",
            City = "Plovdiv",
            Email = "georgi@ecotribe.bg",
            Skills = "Photography, Social Media",
            PreferredEvents = "Recycling Campaigns",
            Number = "0899112233",
            Instagram = "@georgi.photo",
            Facebook = "facebook.com/georgi.ivanov"
        };

        var model = _modelConverter.ConvertToModel<VolunteerViewModel, Volunteer>(viewModel);

        Assert.Multiple(() =>
        {
            Assert.That(model.Id, Is.EqualTo(viewModel.Id));
            Assert.That(model.Name, Is.EqualTo(viewModel.Name));
            Assert.That(model.City, Is.EqualTo(viewModel.City));
            Assert.That(model.Email, Is.EqualTo(viewModel.Email));
            Assert.That(model.Skills, Is.EqualTo(viewModel.Skills));
            Assert.That(model.PreferredEvents, Is.EqualTo(viewModel.PreferredEvents));
            Assert.That(model.Number, Is.EqualTo(viewModel.Number));
            Assert.That(model.Instagram, Is.EqualTo(viewModel.Instagram));
            Assert.That(model.Facebook, Is.EqualTo(viewModel.Facebook));

            Assert.That(model.UserId, Is.Null);
            Assert.That(model.DateOfBirth, Is.EqualTo(default(DateTime)));
            Assert.That(model.User, Is.Null);
            Assert.That(model.EventVolunteers, Is.Null.Or.Empty);
            Assert.That(model.Feedbacks, Is.Null.Or.Empty);
        });
    }
}
