using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LearnWithMentor.DAL.Entities;

namespace LearnWithMentor.DAL.Repositories.Interfaces
{
    public interface ITaskDiscussionRepository : IRepository<TaskDiscussion>
    {
        Task AddTaskDiscussion(TaskDiscussion taskDiscussion);
        Task<IEnumerable<TaskDiscussion>> GetTaskDiscussionAsync(int taskId);
    }
}
