using EcoTribe.BusinessObjects.InputModels;
using EcoTribe.BusinessObjects.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoTribe.Services.Interfaces
{
    public interface IEventService : IService<EventViewModel, EventInputModel>
    {
        void Create(EventInputModel inputModel);
        void Delete(int id);
        void Update(int id, EventInputModel inputModel);
    }
}
