using EcoTribe.BusinessObjects.Domain.Models;
using EcoTribe.BusinessObjects.InputModels;
using EcoTribe.BusinessObjects.ViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EcoTribe.Services.Interfaces
{
    public interface IEventService : IService<EventViewModel, EventInputModel>
    {
        EventDetailsViewModel GetByIdWithVolunteersAndSponsorsAndFeedbacks(int id);
        void AddSponsor(int eventId, int organizationId);
        List<Organization> GetOrganizations();
        void AddFeedback(FeedbackInputModel inputModel);
        List<FeedbackViewModel> GetFeedbacksForEvent(int eventId);
        void CreateAndNotifyUsers(EventInputModel inputModel);
        IEnumerable<EventViewModel> GetAll(string userId);
        Task AddPhotoAsync(int eventId, IFormFile file);
    }
}
