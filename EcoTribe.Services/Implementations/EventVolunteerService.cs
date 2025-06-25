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
    public class EventVolunteerService : IEventVolunteerService
    {  
        private readonly IAppDbContext context;
        private readonly IModelConverter modelConverter;

        public EventVolunteerService(IAppDbContext context, IModelConverter modelConverter)
        {
            this.context = context;
            this.modelConverter = modelConverter;
        }
        
        public IEnumerable<EventVolunteerViewModel> GetAll()
        {
            return context.EventVolunteers.Include(ev => ev.Volunteer).Include(ev => ev.Event)
                .Select(evv => modelConverter.ConvertToViewModel<EventVolunteer, EventVolunteerViewModel>(evv))
                .ToList();
        }
        
        public void Create(EventVolunteerInputModel inputModel)
        {
            var eventVolunteer = modelConverter.ConvertToModel<EventVolunteerInputModel, EventVolunteer>(inputModel);
            context.EventVolunteers.Add(eventVolunteer);
            context.SaveChanges();
        }

        public EventVolunteerViewModel? GetById(int id)
        {
            var eventVolunteer = context.EventVolunteers.Find(id);
            return eventVolunteer != null
                ? modelConverter.ConvertToViewModel<EventVolunteer, EventVolunteerViewModel>(eventVolunteer)
                : null;
        }
        public void Update(int id, EventVolunteerInputModel inputModel)
        {
            var existingEventVolunteer = context.EventVolunteers.Find(id);
            if (existingEventVolunteer == null)
            {
                throw new ArgumentException("EventVolunteer not found.");
            }
            var updatedEventVolunteer = modelConverter.ConvertToModel<EventVolunteerInputModel, EventVolunteer>(inputModel);
            updatedEventVolunteer.Id = id;

            context.Entry(existingEventVolunteer).CurrentValues.SetValues(updatedEventVolunteer);
            context.SaveChanges();
        }

        public void Delete(int id)
        {
            var eventVolunteer = context.EventVolunteers.Find(id);
            if (eventVolunteer == null)
            {
                throw new ArgumentException("EventVolunteer not found.");
            }

            context.EventVolunteers.Remove(eventVolunteer);
            context.SaveChanges();
        }

        public bool HasUserAlreadyParticipated(int eventId, int volunteerId)
        {
            return context.EventVolunteers
                .Any(ev => ev.EventId == eventId && ev.VolunteerId == volunteerId);
        }

        public void Participate(int eventId, int volunteerId, string intention = "Wants to help.")
        {
            if (!HasUserAlreadyParticipated(eventId, volunteerId))
            {
                var eventVolunteer = new EventVolunteer
                {
                    EventId = eventId,
                    VolunteerId = volunteerId,
                    Intention = intention,
                };
                context.EventVolunteers.Add(eventVolunteer);
                context.SaveChanges();
            }
            else
            {
                throw new InvalidOperationException("User has already participated in this event.");
            }
        }

        public void Unparticipate(int eventId, int volunteerId)
        {
            var ev = context.EventVolunteers
                .FirstOrDefault(ev => ev.EventId == eventId && ev.VolunteerId == volunteerId);

            if (ev != null)
            {
                context.EventVolunteers.Remove(ev);
                context.SaveChanges();
            }
        }
    }
}
