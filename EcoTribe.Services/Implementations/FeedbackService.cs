using EcoTribe.BusinessObjects.Domain.Models;
using EcoTribe.BusinessObjects.InputModels;
using EcoTribe.BusinessObjects.ViewModels;
using EcoTribe.Data.Context;
using EcoTribe.Services.Interfaces;
using EcoTribe.Services.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoTribe.Services.Implementations
{
    public class FeedbackService : IFeedbackService
    {
        private readonly AppDbContext context;

        public FeedbackService(AppDbContext context)
        {
            this.context = context;
        }

        public IEnumerable<FeedbackViewModel> GetAll()
        {
            return context.Feedbacks
                .Include(fb => fb.Event)
                .Include(fb => fb.Volunteer)
                .AsEnumerable()
                .Select(fb =>
                {
                    var viewModel = ModelConverter.ConvertToViewModel<Feedback, FeedbackViewModel>(fb);
                    viewModel.Event = fb.Event;
                    viewModel.Volunteer = fb.Volunteer;
                    viewModel.VolunteerName = fb.Volunteer.Name; 
                    return viewModel;
                })
                .ToList();
        }

        public void Create(FeedbackInputModel inputModel)
        {
            var feedback = ModelConverter.ConvertToModel<FeedbackInputModel, Feedback>(inputModel);
            feedback.Event = context.Events.Find(inputModel.EventId)!;
            feedback.Volunteer = context.Volunteers.Include(v => v.User).FirstOrDefault(v => inputModel.VolunteerId == v.Id)!;
            feedback.CreatedAt = DateTime.Now;
            context.Feedbacks.Add(feedback);
            context.SaveChanges();
        }

        public FeedbackViewModel? GetById(int id)
        {
            var feedback = context.Feedbacks
            .Include(fb => fb.Event)
            .Include(fb => fb.Volunteer)
            .FirstOrDefault(fb => fb.Id == id);

            if (feedback == null) return null;

            var viewModel = ModelConverter.ConvertToViewModel<Feedback, FeedbackViewModel>(feedback);
            viewModel.Event = feedback.Event;
            viewModel.Volunteer = feedback.Volunteer;

            return viewModel;
        }
        public void Update(int id, FeedbackInputModel inputModel)
        {
            var existingFeedback = context.Feedbacks
            .Include(fb => fb.Event)
            .Include(fb => fb.Volunteer)
            .FirstOrDefault(fb => fb.Id == id);

            if (existingFeedback == null)
            {
                throw new ArgumentException("Feedback not found.");
            }

            var updatedFeedback = ModelConverter.ConvertToModel<FeedbackInputModel, Feedback>(inputModel);
            updatedFeedback.Id = existingFeedback.Id;
            updatedFeedback.Event = context.Events.Find(inputModel.EventId)!;
            updatedFeedback.Volunteer = context.Volunteers.Find(inputModel.VolunteerId)!;

            context.Entry(existingFeedback).CurrentValues.SetValues(updatedFeedback);
            context.SaveChanges();
        }
        public void Delete(int id)
        {
            var feedback = context.Feedbacks.Find(id);
            if (feedback == null)
            {
                throw new ArgumentException("Feedback not found.");
            }

            context.Feedbacks.Remove(feedback);
            context.SaveChanges();
        }

        public List<EventViewModel> GetAllEvents()
        {
            return context.Events
                .Select(e => new EventViewModel { Id = e.Id, Name = e.Name })
                .ToList();
        }

        public List<VolunteerViewModel> GetAllVolunteers()
        {
            return context.Volunteers
                .Select(v => new VolunteerViewModel { Id = v.Id, Name = v.Name })
                .ToList();
        }

        public bool HasUserProvidedFeedback(int eventId, string userId)
        {
            return context.Feedbacks.Include(f => f.Volunteer)
                .Any(f => f.EventId == eventId && f.Volunteer.UserId == userId);
        }

        public bool HasEventStarted(int eventId)
        {
            throw new NotImplementedException();
        }
    }
}
