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
    public class VolunteerService : IVolunteerService
    {
        private readonly IAppDbContext context;
        private readonly IModelConverter modelConverter;

        public VolunteerService(IAppDbContext context, IModelConverter modelConverter)
        {
            this.context = context;
            this.modelConverter = modelConverter;
        }

        public IEnumerable<VolunteerViewModel> GetAll()
        {
            return context.Volunteers
                .Select(vol => modelConverter.ConvertToViewModel<Volunteer, VolunteerViewModel>(vol))
                .ToList();
        }

        public void Create(VolunteerInputModel inputModel)
        {
            var volunteer = modelConverter.ConvertToModel<VolunteerInputModel, Volunteer>(inputModel);
            context.Volunteers.Add(volunteer);
            context.SaveChanges();
        }

        public VolunteerViewModel? GetById(int id)
        {
            var volunteer = context.Volunteers.Find(id);
            return volunteer != null
                ? modelConverter.ConvertToViewModel<Volunteer, VolunteerViewModel>(volunteer)
                : null;
        }

        public void Update(int id, VolunteerInputModel inputModel)
        {
            var existingVolunteer = context.Volunteers.Find(id);
            if (existingVolunteer == null)
            {
                throw new ArgumentException("Volunteer not found.");
            }
            var updatedVolunteer = modelConverter.ConvertToModel<VolunteerInputModel, Volunteer>(inputModel);
            updatedVolunteer.Id = id;

            context.Entry(existingVolunteer).CurrentValues.SetValues(updatedVolunteer);
            context.SaveChanges();
        }

        public void Delete(int id)
        {
            var volunteer = context.Volunteers.Find(id);
            if (volunteer == null)
            {
                throw new ArgumentException("Volunteer not found.");
            }
            context.Volunteers.Remove(volunteer);
            context.SaveChanges();
        }

        public Volunteer? GetByUserId(string userId)
        {
            return context.Volunteers.FirstOrDefault(v => v.UserId == userId);
        }
    }
}
