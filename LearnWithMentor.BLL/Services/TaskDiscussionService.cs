using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LearnWithMentor.BLL.Interfaces;
using LearnWithMentor.DAL.Entities;
using LearnWithMentor.DAL.UnitOfWork;
using LearnWithMentorBLL.Services;
using LearnWithMentorDTO;

namespace LearnWithMentor.BLL.Services
{
    public class TaskDiscussionService : BaseService, ITaskDiscussionService
    {
        public TaskDiscussionService(IUnitOfWork db) : base(db)
        {

        }

        public async Task AddTaskDiscussionAsync(int senderId, int taskId, string text, DateTime datePosted)
        {
            var taskDiscussion = new TaskDiscussion
            {
                SenderId = senderId,
                TaskId = taskId,
                Text = text,
                DatePosted = datePosted
            };

            await db.TaskDiscussion.AddTaskDiscussion(taskDiscussion);
        }

        public async Task<IEnumerable<TaskDiscussionDTO>> GetTaskDiscussionAsync(int taskId)
        {
            var taskDiscussion = await db.TaskDiscussion.GetTaskDiscussionAsync(taskId);
            var taskDiscussionDtoList = taskDiscussion.Select(n =>
                new TaskDiscussionDTO(
                    n.Id,
                    n.SenderId,
                    n.TaskId,
                    n.Text,
                    n.DatePosted));
            return taskDiscussionDtoList;
        }
    }
}
