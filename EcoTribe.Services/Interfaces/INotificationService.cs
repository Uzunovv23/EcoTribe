using EcoTribe.BusinessObjects.Domain.Models;
using EcoTribe.BusinessObjects.InputModels;
using EcoTribe.BusinessObjects.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoTribe.Services.Interfaces
{
    public interface INotificationService : IService<NotificationViewModel, NotificationInputModel>
    {
        Task<int> GetUnreadCountAsync(string userId);
        Task<IEnumerable<NotificationViewModel>> GetUserUnreadNotifications(string userId); 
        Task<IEnumerable<NotificationViewModel>> GetUserNotificationsAsync(string userId);
        Task MarkAsReadAsync(int notificationId);
        Task CreateEventReminderNotificationsAsync();
        Task MarkUserNotificationsAsReadAsync(string userId);
    }
}
