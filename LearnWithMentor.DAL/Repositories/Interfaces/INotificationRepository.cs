﻿using LearnWithMentor.DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LearnWithMentor.DAL.Repositories.Interfaces
{
    public interface INotificationRepository : IRepository<Notification>
    {
        Task AddNotificationAsync(Notification notification);
        Task MarkNotificationsAsReadAsync(IEnumerable<int> idList);
        Task<IEnumerable<Notification>> GetNotificationsAsync(int userId, int amount);
    }
}