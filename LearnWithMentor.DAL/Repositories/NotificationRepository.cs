using LearnWithMentor.DAL.Entities;
using Microsoft.EntityFrameworkCore;
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
    }
}
