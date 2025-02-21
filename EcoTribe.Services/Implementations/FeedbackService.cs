using EcoTribe.BusinessObjects.Domain.Models;
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

        public FeedbackViewModel? GetById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
