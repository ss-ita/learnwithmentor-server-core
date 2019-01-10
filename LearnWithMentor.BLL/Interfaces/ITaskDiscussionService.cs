using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LearnWithMentorDTO;

namespace LearnWithMentor.BLL.Interfaces
{
    public interface ITaskDiscussionService
    {
        Task AddTaskDiscussionAsync(int senderId, int taskId, string text, DateTime datePosted);
        Task<IEnumerable<TaskDiscussionDTO>> GetTaskDiscussionAsync(int taskId);
    }
}
