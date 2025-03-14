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
        private readonly AppDbContext context;

        public EventService(AppDbContext context)
        {
            this.context = context;
        }
        public IEnumerable<EventViewModel> GetAll()
        {
            return context.Events
                .Select(ev => ModelConverter.ConvertToViewModel<Event, EventViewModel>(ev))
                .ToList();
        }

        public void Create(EventInputModel inputModel)
        {
            var eventEntity = ModelConverter.ConvertToModel<EventInputModel, Event>(inputModel);
            context.Events.Add(eventEntity);
            context.SaveChanges();
        }       
        public EventViewModel? GetById(int id)
        {
            var eventEntity = context.Events.Find(id);
            return eventEntity != null
                ? ModelConverter.ConvertToViewModel<Event, EventViewModel> (eventEntity)
                : null;
        }

        public EventDetailsViewModel? GetByIdWithVolunteersAndSponsors(int id)
        {
            var eventEntity = context.Events
                .Include(e => e.EventSponsors)
                    .ThenInclude(es => es.Organization)
                .Include(e => e.EventVolunteers)
                    .ThenInclude(ev => ev.Volunteer)
                .FirstOrDefault(e => e.Id == id);

            if (eventEntity == null)
            {
                return null;
            }

            var eventDetailsViewModel = ModelConverter.ConvertToViewModel<Event, EventDetailsViewModel>(eventEntity);

            eventDetailsViewModel.AttendingVolunteers = eventEntity.EventVolunteers
                .Select(ev => ModelConverter.ConvertToViewModel<Volunteer, VolunteerViewModel>(ev.Volunteer))
                .ToList();


            eventDetailsViewModel.Sponsors = eventEntity.EventSponsors
                .Select(es => ModelConverter.ConvertToViewModel<Organization, EventSponsorViewModel>(es.Organization))
                .ToList();

            return eventDetailsViewModel;
        }

        public void Update(int id, EventInputModel inputModel)
        {
            var existingEvent = context.Events.Find(id);
            if (existingEvent == null)
            {
                throw new ArgumentException("Event not found.");
            }

            var updatedEvent = ModelConverter.ConvertToModel<EventInputModel, Event> (inputModel);
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


    }
}
