using EcoTribe.BusinessObjects.InputModels;
using EcoTribe.BusinessObjects.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoTribe.Services.Interfaces
{
    public interface IEventVolunteerService : IService<EventVolunteerViewModel, EventVolunteerInputModel>
    {
        void Create(EventVolunteerInputModel inputModel);
        void Update(int id, EventVolunteerInputModel inputModel);
        void Delete (int id);
    }
}
