using System.Collections.Generic;
using System.Threading.Tasks;
using LearnWithMentor.DAL.Entities;
using LearnWithMentorDTO;

namespace LearnWithMentorBLL.Interfaces
{
    public interface INotificationService:IDisposableService
    {
        Task<bool> AddNotificationAsync(string text, string type, int userId);
        void MarkNotificationsAsRead(IEnumerable<Notification> notifications);
        Task<IEnumerable<Notification>> GetNotificationsByUserIdAsync(int userId, int amount);
    }
}
