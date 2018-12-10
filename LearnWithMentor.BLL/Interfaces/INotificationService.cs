using LearnWithMentorDTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LearnWithMentorBLL.Interfaces
{
    public interface INotificationService : IDisposableService
    {
        Task AddNotificationAsync(string text, string type, DateTime dateTime, int userId);
        Task MarkNotificationsAsReadAsync(IEnumerable<int> idList);
        Task<IEnumerable<NotificationDTO>> GetNotificationsAsync(int userId, int amount);
    }
}
