using EcoTribe.BusinessObjects.Domain.Models;
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
    }
}
