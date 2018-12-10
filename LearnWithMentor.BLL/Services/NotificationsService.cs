using LearnWithMentor.DAL.Entities;
using LearnWithMentor.DAL.UnitOfWork;
using LearnWithMentorBLL.Interfaces;
using LearnWithMentorDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LearnWithMentorBLL.Services
{
    public class NotificationsService : BaseService, INotificationService
    {
        public NotificationsService(IUnitOfWork db) : base(db)
        {

        }

        public async Task AddNotificationAsync(string text, string type, DateTime dateTime, int userId)
        {
            if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(type))
                return;

            var notification = new Notification
            {
                Text = text,
                Type = type,
                UserId = userId,
                DateTime = dateTime,
                IsRead = false
            };

            await db.Notification.AddNotificationAsync(notification);
            db.Save();
        }

        public async Task MarkNotificationsAsReadAsync(IEnumerable<int> idList)
        {
            await db.Notification.MarkNotificationsAsReadAsync(idList);
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
