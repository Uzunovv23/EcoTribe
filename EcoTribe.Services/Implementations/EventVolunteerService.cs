using EcoTribe.BusinessObjects.Domain.Models;
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

        public EventVolunteerViewModel? GetById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
