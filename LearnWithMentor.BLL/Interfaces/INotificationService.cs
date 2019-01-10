using LearnWithMentorDTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LearnWithMentorBLL.Interfaces
{
    public interface INotificationService : IDisposableService
    {
        Task AddNotificationAsync(string text, NotificationType type, DateTime dateTime, int userId);
        Task MarkAllNotificationsAsReadAsync(int userId);
        Task<IEnumerable<NotificationDTO>> GetNotificationsAsync(int userId, int amount);
    }
}
