using System.Collections.Generic;
using LearnWithMentorDTO;
using LearnWithMentor.DAL.Entities;
using LearnWithMentorBLL.Interfaces;
using LearnWithMentor.DAL.UnitOfWork;
using System.Threading.Tasks;

namespace LearnWithMentorBLL.Services
{
    class NotificationsService : BaseService,INotificationService
    {
        public NotificationsService(IUnitOfWork db) : base(db)
        {

        }

        public async Task<bool> AddNotificationAsync(string text, string status, int userId)
        {
            if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(status))
                return false;
            var notificationNew = new Notification
            {
                Text = text,
                Status = status,
                UserId = userId
            };
            await db.Notification.AddAsync(notificationNew);
            db.Save();
            return true;
        }

        public void MarkNotificationsAsRead(IEnumerable<Notification> notifications)
        {
            db.Notification.MarkNotificationAsReadAsync(notifications);
        }

        public async Task<IEnumerable<Notification>> GetNotificationsByUserIdAsync(int userId, int amount)
        {
            var notificationsByUser = await db.Notification.GetNotificationsAsync(userId, amount);
            return notificationsByUser;
        }
    }
}
