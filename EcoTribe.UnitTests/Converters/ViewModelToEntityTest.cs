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
}
