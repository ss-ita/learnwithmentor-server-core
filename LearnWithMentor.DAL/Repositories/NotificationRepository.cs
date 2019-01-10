using LearnWithMentor.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LearnWithMentor.DAL.Repositories.Interfaces
{
    public class NotificationRepository : BaseRepository<Notification>, INotificationRepository
    {
        public NotificationRepository(LearnWithMentorContext context) : base(context)
        {

        }

        public async Task AddNotificationAsync(Notification notification)
        {
            bool userExist = await Context.Users.AnyAsync(u => u.Id == notification.UserId);

            if (userExist)
            {
                await Context.AddAsync(notification);
            }
        }

        public async Task MarkAllNotificationsAsReadAsync(int userId)
        {
            await Context.Notifications
                .Where(notification => notification.UserId == userId && !notification.IsRead)
                .ForEachAsync(notification => notification.IsRead = true);
        }

        public async Task<IEnumerable<Notification>> GetNotificationsAsync(int userId, int amount)
        {
            return await Context.Notifications
                .Where(notification => notification.UserId == userId)
                .OrderByDescending(notification => notification.DateTime)
                .Take(amount)
                .ToListAsync();
        }

        public async Task<Notification> GetLastUnreadNotificationByType(int userId, string type)
        {
            return await Context.Notifications
                .Where(notification => notification.UserId == userId && notification.Type.ToString() == type && !notification.IsRead)
                .OrderByDescending(notification => notification.DateTime)
                .FirstOrDefaultAsync();
        }

        public async Task UpdateNotificationTime(int notificationId, DateTime newTime)
        {
            await Context.Notifications
                .Where(notification => notification.Id == notificationId)
                .ForEachAsync(notification => notification.DateTime = newTime);
        }
    }
}
