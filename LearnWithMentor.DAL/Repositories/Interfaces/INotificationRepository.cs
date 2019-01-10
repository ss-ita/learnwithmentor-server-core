using LearnWithMentor.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LearnWithMentor.DAL.Repositories.Interfaces
{
    public interface INotificationRepository : IRepository<Notification>
    {
        Task AddNotificationAsync(Notification notification);
        Task MarkAllNotificationsAsReadAsync(int userId);
        Task<IEnumerable<Notification>> GetNotificationsAsync(int userId, int amount);
        Task<Notification> GetLastUnreadNotificationByType(int userId, string type);
        Task UpdateNotificationTime(int notificationId, DateTime newTime);
    }
}
