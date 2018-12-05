using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LearnWithMentor.DAL.Entities;
using LearnWithMentor.DAL.Repositories.Interfaces;
using System.Collections.Generic;
using Task = System.Threading.Tasks.Task;

namespace LearnWithMentor.DAL.Repositories.Interfaces
{
    public class NotificationRepository : BaseRepository<Notification>, INotificationRepository
    {
        public NotificationRepository(LearnWithMentorContext context) : base(context)
        {

        }
    }
}
