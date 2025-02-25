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
        private readonly AppDbContext context;

        public EventVolunteerService(AppDbContext context)
        {
            this.context = context;
        }
        
        public IEnumerable<EventVolunteerViewModel> GetAll()
        {
            return context.EventVolunteers.Include(ev => ev.Volunteer).Include(ev => ev.Event)
                .Select(evv => ModelConverter.ConvertToViewModel<EventVolunteer, EventVolunteerViewModel>(evv))
                .ToList();
        }
        
        public void Create(EventVolunteerInputModel inputModel)
        {
            var eventVolunteer = ModelConverter.ConvertToModel<EventVolunteerInputModel, EventVolunteer>(inputModel);
            context.EventVolunteers.Add(eventVolunteer);
            context.SaveChanges();
        }

        public EventVolunteerViewModel? GetById(int id)
        {
            var eventVolunteer = context.EventVolunteers.Find(id);
            return eventVolunteer != null
                ? ModelConverter.ConvertToViewModel<EventVolunteer, EventVolunteerViewModel>(eventVolunteer)
                : null;
        }
        public void Update(int id, EventVolunteerInputModel inputModel)
        {
            var existingEventVolunteer = context.EventVolunteers.Find(id);
            if (existingEventVolunteer == null)
            {
                throw new ArgumentException("EventVolunteer not found.");
            }
            var updatedEventVolunteer = ModelConverter.ConvertToModel<EventVolunteerInputModel, EventVolunteer>(inputModel);
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
    }
}
