using EcoTribe.BusinessObjects.InputModels;
using EcoTribe.BusinessObjects.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoTribe.Services.Interfaces
{
    public interface ILocationService 
    {
        IEnumerable<LocationViewModel> GetAll();
        LocationViewModel? GetById(int id);
        void Create(LocationInputModel inputModel);
        void Update(int id, LocationInputModel inputModel);
    }
}
