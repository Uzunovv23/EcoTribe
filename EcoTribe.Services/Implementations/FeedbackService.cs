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
            return context.Feedbacks.Include(fb => fb.Volunteer).Include(fb => fb.Event)
                .Select(feedb => ModelConverter.ConvertToViewModel<Feedback, FeedbackViewModel>(feedb))
                .ToList();
        }

        public void Create(FeedbackInputModel inputModel)
        {
            var feedback = ModelConverter.ConvertToModel<FeedbackInputModel, Feedback>(inputModel);
            context.Feedbacks.Add(feedback);
            context.SaveChanges();
        }

        public FeedbackViewModel? GetById(int id)
        {
            var feedback = context.Feedbacks.Find(id);
            return feedback != null
                ? ModelConverter.ConvertToViewModel<Feedback, FeedbackViewModel>(feedback)
                : null;
        }
        public void Update(int id, FeedbackInputModel inputModel)
        {
            var existingFeedback = context.Feedbacks.Find(id);
            if (existingFeedback == null)
            {
                throw new ArgumentException("Feedback not found.");
            }

            var updatedFeedback = ModelConverter.ConvertToModel<FeedbackInputModel, Feedback>(inputModel);
            updatedFeedback.Id = existingFeedback.Id;

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

    }
}
