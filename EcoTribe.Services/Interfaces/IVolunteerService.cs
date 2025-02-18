using EcoTribe.BusinessObjects.InputModels;
using EcoTribe.BusinessObjects.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoTribe.Services.Interfaces
{
    public interface IVolunteerService
    {
        IEnumerable<VolunteerViewModel> GetAll();
        void Create(VolunteerInputModel inputModel);
    }
}
