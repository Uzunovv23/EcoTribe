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

    public class EventSponsorService : IEventSponsorService
    {
        private readonly IAppDbContext context;
        private readonly IModelConverter modelConverter;

        public EventSponsorService(IAppDbContext context, IModelConverter modelConverter)
        {
            this.context = context;
            this.modelConverter = modelConverter;
        }

        public IEnumerable<EventSponsorViewModel> GetAll()
        {
            return context.EventSponsors
                .Include(es => es.Event)
                .Include(es => es.Organization)
                .Select(es => modelConverter.ConvertToViewModel<EventSponsor, EventSponsorViewModel>(es))
                .ToList();
        }

        public void Create(EventSponsorInputModel inputModel)
        {
            var eventExists = context.Events.Any(e => e.Id == inputModel.EventId);
            var organizationExists = context.Organizations.Any(o => o.Id == inputModel.OrganizationId);

            if (!eventExists || !organizationExists)
            {
                throw new ArgumentException("The specified Event or Organization does not exist.");
            }

            var eventSponsor = new EventSponsor
            {
                EventId = inputModel.EventId,
                OrganizationId = inputModel.OrganizationId
            };

            context.EventSponsors.Add(eventSponsor);

            context.SaveChanges();
        }

        public EventSponsorViewModel? GetById(int eventId, int organizationId)
        {
            var eventSponsor = context.EventSponsors
                .Include(es => es.Event)
                .Include(es => es.Organization)
                .FirstOrDefault(es => es.EventId == eventId && es.OrganizationId == organizationId);

            if (eventSponsor == null)
            {
                return null;
            }

            var viewModel = modelConverter.ConvertToViewModel<EventSponsor, EventSponsorViewModel>(eventSponsor);

            viewModel.Event = eventSponsor.Event;
            viewModel.Organization = eventSponsor.Organization;

            return viewModel;
        }

        public void Update(int id, EventSponsorInputModel inputModel)
        {
            var existingEventSponsor = context.EventSponsors
                .Include(es => es.Event)
                .Include(es => es.Organization)
                .FirstOrDefault(es => es.Id == id);

            if (existingEventSponsor == null)
            {
                throw new ArgumentException("Event sponsor not found.");
            }

            var updatedEventSponsor = modelConverter.ConvertToModel<EventSponsorInputModel, EventSponsor>(inputModel);
            updatedEventSponsor.Id = id;

            context.Entry(existingEventSponsor).CurrentValues.SetValues(updatedEventSponsor);
            context.SaveChanges();
        }

        public void Delete(int eventId, int organizationId)
        {
            var eventSponsor = context.EventSponsors
                .FirstOrDefault(es => es.EventId == eventId && es.OrganizationId == organizationId);

            if (eventSponsor != null)
            {
                context.EventSponsors.Remove(eventSponsor);
                context.SaveChanges();
            }
        }

        public EventSponsorViewModel? GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }
    }

}
