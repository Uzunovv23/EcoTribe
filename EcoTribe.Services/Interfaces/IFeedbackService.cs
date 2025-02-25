using EcoTribe.BusinessObjects.InputModels;
using EcoTribe.BusinessObjects.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoTribe.Services.Interfaces
{
    public interface IFeedbackService : IService<FeedbackViewModel, FeedbackInputModel>
    {
        void Create(FeedbackInputModel inputModel);
        void Update(int id, FeedbackInputModel inputModel);
        void Delete(int id);
        List<EventViewModel> GetAllEvents();
        List<VolunteerViewModel> GetAllVolunteers();
    }
}
