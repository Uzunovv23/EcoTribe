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

    }
}
