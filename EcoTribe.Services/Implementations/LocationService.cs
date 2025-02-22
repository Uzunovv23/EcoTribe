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
    public class LocationService : ILocationService
    {
        private readonly AppDbContext context;

        public LocationService(AppDbContext context)
        {
            this.context = context;
        }

        public IEnumerable<LocationViewModel> GetAll()
        {
            return context.Locations
                .Select(loc => ModelConverter.ConvertToViewModel<Location, LocationViewModel>(loc))
                .ToList();
        }

        public void Create(LocationInputModel inputModel)
        {
            var location = ModelConverter.ConvertToModel<LocationInputModel, Location>(inputModel);
            context.Locations.Add(location);
            context.SaveChanges();
        }
        public LocationViewModel? GetById(int id)
        {
            var location = context.Locations.Find(id);
            return location != null
                ? ModelConverter.ConvertToViewModel<Location, LocationViewModel>(location)
                : null;
        }
        public void Update(int id, LocationInputModel inputModel)
        {
            var existingLocation = context.Locations.Find(id);
            if (existingLocation == null)
            {
                throw new ArgumentException("Location not found.");
            }

            var updatedLocation = ModelConverter.ConvertToModel<LocationInputModel, Location>(inputModel);
            updatedLocation.Id = id; 

            context.Entry(existingLocation).CurrentValues.SetValues(updatedLocation);

            context.SaveChanges();
        }
        public void Delete(int id)
        {
            var location = context.Locations.Find(id);
            if (location == null)
            {
                throw new ArgumentException("Event not found.");
            }

            context.Locations.Remove(location);
            context.SaveChanges();
        }

    }
}
