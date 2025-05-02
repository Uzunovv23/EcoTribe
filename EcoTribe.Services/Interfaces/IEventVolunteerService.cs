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
        bool HasUserAlreadyParticipated(int eventId, int volunteerId);
        void Participate(int eventId, int volunteerId, string intention = "Wants to help.");
    }
}
