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
    public class NotificationService : INotificationService
    {
        private readonly AppDbContext context;

        public NotificationService(AppDbContext context)
        {
            this.context = context;
        }

        public async Task MarkAsReadAsync(int notificationId)
        {
            var notification = await context.Notifications.FindAsync(notificationId);
            if (notification != null)
            {
                notification.IsRead = true;
                await context.SaveChangesAsync();
            }
        }

        public async Task CreateEventReminderNotificationsAsync()
        {
            var now = DateTime.UtcNow;
            var in24Hours = now.AddHours(24);

            // 1. Get events starting within the next 24 hours
            var upcomingEvents = await context.Events
                .Where(e => e.Start > now && e.Start <= in24Hours)
                .Include(e => e.EventVolunteers)
                    .ThenInclude(ev => ev.Volunteer)
                        .ThenInclude(v => v.User)
                .ToListAsync();

            foreach (var ev in upcomingEvents)
            {
                foreach (var evVol in ev.EventVolunteers)
                {
                    var volunteer = evVol.Volunteer;
                    var userId = volunteer.UserId;

                    if (string.IsNullOrEmpty(userId))
                        continue;

                    // 2. Avoid duplicate notifications
                    bool exists = await context.Notifications.AnyAsync(n =>
                        n.UserId == userId &&
                        n.EventId == ev.Id &&
                        n.Message.Contains("starts in less than 24 hours"));

                    if (!exists)
                    {
                        context.Notifications.Add(new Notification
                        {
                            UserId = userId,
                            EventId = ev.Id,
                            Title = "Upcoming Event Reminder",
                            Message = $"The event \"{ev.Name}\" starts in less than 24 hours.",
                            CreatedAt = DateTime.UtcNow,
                            IsRead = false
                        });
                    }
                }
            }

            await context.SaveChangesAsync();
        }

        public async Task<int> GetUnreadCountAsync(string userId)
        {
            return await context.Notifications
                .Where(n => n.UserId == userId && !n.IsRead)
                .CountAsync();
        }

        public async Task<IEnumerable<Notification>> GetUserNotificationsAsync(string userId)
        {
            return await context.Notifications
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        IEnumerable<NotificationViewModel> IService<NotificationViewModel, NotificationInputModel>.GetAll()
        {
            throw new NotImplementedException();
        }

        NotificationViewModel? IService<NotificationViewModel, NotificationInputModel>.GetById(int id)
        {
            throw new NotImplementedException();
        }

        void IService<NotificationViewModel, NotificationInputModel>.Create(NotificationInputModel inputModel)
        {
            throw new NotImplementedException();
        }

        void IService<NotificationViewModel, NotificationInputModel>.Update(int id, NotificationInputModel inputModel)
        {
            throw new NotImplementedException();
        }

        void IService<NotificationViewModel, NotificationInputModel>.Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
