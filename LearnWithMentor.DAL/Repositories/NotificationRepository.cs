using System.Linq;
using LearnWithMentor.DAL.Entities;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using LearnWithMentor.DAL.Repositories.Interfaces;
using ThreadTask = System.Threading.Tasks;

namespace LearnWithMentor.DAL.Repositories.Interfaces
{
    public class NotificationRepository : BaseRepository<Notification>, INotificationRepository
    {
        public NotificationRepository(LearnWithMentorContext context) : base(context)
        {

        }

        public Notification AddNotificationAsync(Notification notification)
        {
            Context.Add(notification);
            return notification;
        }

        public async Task<IEnumerable<Notification>> GetNotificationsAsync(int userId, int amount)
        {
            return await Context.Notifications.Where(notification => notification.UserId == userId).Take(amount).ToListAsync();
        }

        public void MarkNotificationAsReadAsync(IEnumerable<Notification> notifications)
        {
            Context.Notifications.Where(notification => notifications.Select(receiptNotifications => receiptNotifications.Id)
                                                                      .Contains(notification.Id)).
                                                                      ForEachAsync(notification => notification.IsRead = true);
        }
    }
}
