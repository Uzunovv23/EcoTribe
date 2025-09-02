using EcoTribe.BusinessObjects.Domain.Models;
using EcoTribe.BusinessObjects.InputModels;
using EcoTribe.BusinessObjects.ViewModels;
using EcoTribe.Data.Context;
using EcoTribe.Services.Interfaces;
using EcoTribe.Services.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoTribe.Services.Implementations
{
    public class EventService : IEventService
    {
        private readonly IAppDbContext context;
        private readonly IModelConverter modelConverter;
        private readonly IOrganizationService organizationService;

        public EventService(IAppDbContext context, IModelConverter modelConverter, IOrganizationService organizationService)
        {
            this.context = context;
            this.modelConverter = modelConverter;
            this.organizationService = organizationService;
        }
        public IEnumerable<EventViewModel> GetAll()
        {
            return context.Events
                .Select(ev => modelConverter.ConvertToViewModel<Event, EventViewModel>(ev))
                .ToList();
        }

        public void Create(EventInputModel inputModel)
        {
            var eventEntity = modelConverter.ConvertToModel<EventInputModel, Event>(inputModel);
            eventEntity.CreatedBy = inputModel.CreatedBy;
            context.Events.Add(eventEntity);
            context.SaveChanges();
        }       
        public EventViewModel? GetById(int id)
        {
            var eventEntity = context.Events.Find(id);
            return eventEntity != null
                ? modelConverter.ConvertToViewModel<Event, EventViewModel> (eventEntity)
                : null;
        }

        public EventDetailsViewModel? GetByIdWithVolunteersAndSponsorsAndFeedbacks(int id)
        {
            var eventEntity = context.Events
                .Include(e => e.EventSponsors)
                    .ThenInclude(es => es.Organization)
                .Include(e => e.EventVolunteers)
                    .ThenInclude(ev => ev.Volunteer)
                .Include(e => e.Feedbacks)
                    .ThenInclude(f => f.Volunteer)
                .FirstOrDefault(e => e.Id == id);

            if (eventEntity == null)
            {
                return null;
            }

            var eventDetailsViewModel = modelConverter.ConvertToViewModel<Event, EventDetailsViewModel>(eventEntity);

            eventDetailsViewModel.AttendingVolunteers = eventEntity.EventVolunteers
                .Select(ev => modelConverter.ConvertToViewModel<Volunteer, VolunteerViewModel>(ev.Volunteer))
                .ToList();

            eventDetailsViewModel.Sponsors = eventEntity.EventSponsors
                .Select(es => modelConverter.ConvertToViewModel<Organization, EventSponsorViewModel>(es.Organization))
                .ToList();

            eventDetailsViewModel.Feedbacks = eventEntity.Feedbacks
                .Select(f =>
                {
                    var feedbackVM = modelConverter.ConvertToViewModel<Feedback, FeedbackViewModel>(f);
                    feedbackVM.VolunteerName = f.Volunteer.Name;
                    return feedbackVM;
                })
                .ToList();

            eventDetailsViewModel.VolunteerId = null;

            return eventDetailsViewModel;
        }

        public void Update(int id, EventInputModel inputModel)
        {
            var existingEvent = context.Events.Find(id);
            if (existingEvent == null)
            {
                throw new ArgumentException("Event not found.");
            }

            var updatedEvent = modelConverter.ConvertToModel<EventInputModel, Event> (inputModel);
            updatedEvent.Id = id;

            context.Entry(existingEvent).CurrentValues.SetValues(updatedEvent);
            context.SaveChanges();            
        }

        public void Delete(int id)
        {
            var eventEntity = context.Events.Find(id);
            if (eventEntity == null)
            {
                throw new ArgumentException("Event not found.");
            }

            context.Events.Remove(eventEntity);
            context.SaveChanges();
        }

        public List<Organization> GetOrganizations()
        {
            return context.Organizations.ToList(); 
        }

        public void AddSponsor(int eventId, int organizationId)
        {
            var eventEntity = context.Events
                .Include(e => e.EventSponsors) 
                .FirstOrDefault(e => e.Id == eventId);

            var organization = context.Organizations
                .FirstOrDefault(o => o.Id == organizationId);

            if (eventEntity == null || organization == null)
            {
                throw new ArgumentException("Invalid event or organization.");
            }

            bool alreadyExists = eventEntity.EventSponsors
                .Any(es => es.OrganizationId == organizationId);

            if (!alreadyExists)
            {
                var eventSponsor = new EventSponsor
                {
                    EventId = eventId,
                    OrganizationId = organizationId
                };

                eventEntity.EventSponsors.Add(eventSponsor);
                context.SaveChanges();
            }
        }

        public void AddFeedback(FeedbackInputModel inputModel)
        {
            if (!context.Events.Any(e => e.Id == inputModel.EventId))
            {
                throw new ArgumentException("Invalid Event ID.");
            }

            if (!context.Volunteers.Any(v => v.Id == inputModel.VolunteerId))
            {
                throw new ArgumentException("Invalid Volunteer ID.");
            }

            var feedback = modelConverter.ConvertToModel<FeedbackInputModel, Feedback>(inputModel);
            var volunteer = context.Volunteers.FirstOrDefault(v => v.Id == inputModel.VolunteerId);
            var user = 
            feedback.Volunteer.User = context.Users.FirstOrDefault(u => u.Id == volunteer.UserId);
            context.Feedbacks.Add(feedback);
            context.SaveChanges();
        }

        public List<FeedbackViewModel> GetFeedbacksForEvent(int eventId)
        {
            return context.Feedbacks
                .Where(f => f.EventId == eventId)
                .Select(f => modelConverter.ConvertToViewModel<Feedback, FeedbackViewModel>(f))
                .ToList();
        }

        public void CreateAndNotifyUsers(EventInputModel inputModel)
        {
            var eventEntity = modelConverter.ConvertToModel<EventInputModel, Event>(inputModel);
            eventEntity.CreatedBy = inputModel.CreatedBy;
            context.Events.Add(eventEntity);
            context.SaveChanges(); 

            var allUserIds = context.Users.Select(u => u.Id).ToList();

            foreach (var userId in allUserIds)
            {
                var notification = new Notification
                {
                    UserId = userId,
                    EventId = eventEntity.Id,
                    Title = "New Event Created",
                    Message = $"A new event \"{eventEntity.Name}\" has been created in {eventEntity.City}.",
                    CreatedAt = DateTime.UtcNow,
                    IsRead = false
                };

                context.Notifications.Add(notification);
            }

            context.SaveChanges(); 
        }

        public async IEnumerable<EventViewModel> GetAll(string userId)
        {
            IEnumerable<EventViewModel> events = GetAll();

            OrganizationViewModel? organization = organizationService.GetByUserId(userId);


        }
    }
}
