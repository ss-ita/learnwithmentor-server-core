using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LearnWithMentor.DAL.Entities;
using Task = System.Threading.Tasks.Task;

namespace LearnWithMentor.DAL.Repositories.Interfaces
{
    public interface INotificationRepository : IRepository<Notification>
    {
        Notification AddNotificationAsync(Notification notification);
        void MarkNotificationAsReadAsync(IEnumerable<Notification> notifications);
        Task<IEnumerable<Notification>> GetNotificationsAsync(int userId,int amount);
    }
}
