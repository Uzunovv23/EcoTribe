﻿using EcoTribe.BusinessObjects.Domain.Models;
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
    public class EventResourceService : IEventResourceService
    {
        private readonly AppDbContext context;

        public EventResourceService(AppDbContext context)
        {
            this.context = context;
        }
        public IEnumerable<EventResourceViewModel> GetAll()
        {
            return context.EventResources
                .Select(evr => ModelConverter.ConvertToViewModel<EventResource, EventResourceViewModel>(evr))
                .ToList();
        }

        public void Create(EventResourceInputModel inputModel)
        {
            var eventResource = ModelConverter.ConvertToModel<EventResourceInputModel, EventResource>(inputModel);
            context.EventResources.Add(eventResource);
            context.SaveChanges();
        }

        public EventResourceViewModel? GetById(int id)
        {
            var eventResource = context.EventResources.Find(id);
            return eventResource != null
                ? ModelConverter.ConvertToViewModel<EventResource, EventResourceViewModel>(eventResource)
                : null;
        }

        public void Update(int id, EventResourceInputModel inputModel)
        {
            var existingEventResource = context.EventResources.Find(id);
            if (existingEventResource == null)
            {
                throw new ArgumentException("Event Resource not found.");
            }

            var updatedEventResource = ModelConverter.ConvertToModel<EventResourceInputModel, EventResource>(inputModel);
            updatedEventResource.Id = id;

            context.Entry(existingEventResource).CurrentValues.SetValues(updatedEventResource);
            context.SaveChanges();
        }

        public void Delete(int id)
        {
            var eventResource = context.EventResources.Find(id);
            if (eventResource == null)
            {
                throw new ArgumentException("Event Resource not found.");
            }

            context.EventResources.Remove(eventResource);
            context.SaveChanges();
        }
    }
}
