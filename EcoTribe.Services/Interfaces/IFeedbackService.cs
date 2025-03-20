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
        List<EventViewModel> GetAllEvents();
        List<VolunteerViewModel> GetAllVolunteers();
        bool HasUserProvidedFeedback(int eventId, string userId);
        bool HasEventStarted(int eventId);

    }
}
