using EcoTribe.BusinessObjects.Domain.Models;
using EcoTribe.BusinessObjects.InputModels;
using EcoTribe.BusinessObjects.ViewModels;
using EcoTribe.Data.Context;
using EcoTribe.Services.Interfaces;
using EcoTribe.Services.Utils;
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
    }
}
