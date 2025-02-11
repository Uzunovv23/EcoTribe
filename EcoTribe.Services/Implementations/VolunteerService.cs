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
    public class VolunteerService : IVolunteerService
    {
        private readonly AppDbContext context;

        public VolunteerService(AppDbContext context)
        {
            this.context = context;
        }

        public IEnumerable<VolunteerViewModel> GetAll()
        {
            return context.Volunteers
                .Select(vol => ModelConverter.ConvertToViewModel<Volunteer, VolunteerViewModel>(vol))
                .ToList();
        }

    }
}
