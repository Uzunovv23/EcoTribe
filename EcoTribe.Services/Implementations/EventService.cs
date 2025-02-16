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
    }
}
