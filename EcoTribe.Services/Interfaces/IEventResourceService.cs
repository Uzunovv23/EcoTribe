using EcoTribe.BusinessObjects.InputModels;
using EcoTribe.BusinessObjects.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoTribe.Services.Interfaces
{
    public interface IEventResourceService : IService<EventResourceViewModel, EventResourceInputModel>
    {
        void Create(EventResourceInputModel inputModel);
        void Update(int id, EventResourceInputModel inputModel);
        void Delete(int id);
    }
}
