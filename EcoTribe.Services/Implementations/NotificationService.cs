using EcoTribe.BusinessObjects.Domain.Models;
using EcoTribe.BusinessObjects.InputModels;
using EcoTribe.BusinessObjects.ViewModels;
using EcoTribe.Data.Context;
using EcoTribe.Services.Interfaces;
using EcoTribe.Services.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoTribe.Services.Implementations
{
    public class NotificationService : INotificationService
    {
        private readonly AppDbContext context;

        public NotificationService(AppDbContext context)
        {
            this.context = context;
        }

        public void Create(NotificationInputModel inputModel)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<NotificationViewModel> GetAll()
        {
            return context.Notifications
                .Select(notf => ModelConverter.ConvertToViewModel<Notification, NotificationViewModel>(notf))
                .ToList();
        }

        public NotificationViewModel? GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(int id, NotificationInputModel inputModel)
        {
            throw new NotImplementedException();
        }
    }
}
