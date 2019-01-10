using LearnWithMentor.DAL.Entities;
using LearnWithMentor.DAL.UnitOfWork;
using LearnWithMentorBLL.Interfaces;
using LearnWithMentorDTO;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LearnWithMentorBLL.Services
{
    public class NotificationService : BaseService, INotificationService
    {
        public NotificationService(IUnitOfWork db) : base(db)
        {

        }
        
        public async Task AddNotificationAsync(string text, NotificationType type, DateTime dateTime, int userId)
        {
            Notification notification = null;

            if (type == NotificationType.NewMessage)
            {
                notification = await db.Notification.GetLastUnreadNotificationByType(userId, NotificationType.NewMessage.ToString());

                if (notification != null)
                {
                    await db.Notification.UpdateNotificationTime(notification.Id, dateTime);
                    db.Save();
                    return;
                }
            }

            notification = new Notification
            {
                Text = text,
                Type = type.ToString(),
                UserId = userId,
                DateTime = dateTime,
                IsRead = false
            };

            await db.Notification.AddNotificationAsync(notification);
            db.Save();
        }

        public async Task MarkAllNotificationsAsReadAsync(int userId)
        {
            await db.Notification.MarkAllNotificationsAsReadAsync(userId);
            db.Save();
        }

        public async Task<IEnumerable<NotificationDTO>> GetNotificationsAsync(int userId, int amount)
        {
            var notifications = await db.Notification.GetNotificationsAsync(userId, amount);
            var notificationsDtoList = notifications.Select(n => 
                new NotificationDTO(
                    n.Id, 
                    n.UserId, 
                    n.IsRead,
                    n.Text, 
                    (NotificationType)Enum.Parse(typeof(NotificationType), n.Type),
                    n.DateTime.ToString("dddd, dd/MM/yyyy, HH:mm:ss")));
            return notificationsDtoList;
        }
    }
}
