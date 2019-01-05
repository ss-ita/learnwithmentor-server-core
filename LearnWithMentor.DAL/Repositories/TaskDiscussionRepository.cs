using LearnWithMentor.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LearnWithMentor.DAL.Repositories.Interfaces;

namespace LearnWithMentor.DAL.Repositories
{
    class TaskDiscussionRepository : BaseRepository<TaskDiscussion>, ITaskDiscussionRepository
    {
        public TaskDiscussionRepository(LearnWithMentorContext context) : base(context)
        {

        }

        public async Task AddTaskDiscussion(TaskDiscussion taskDiscussion)
        {
            await Context.AddAsync(taskDiscussion);
        }

        public async Task<IEnumerable<TaskDiscussion>> GetTaskDiscussionAsync(int taskId)
        {
            return await Context.TaskDiscussions
                .Where(taskDiscussion => taskDiscussion.TaskId == taskId)
                .OrderByDescending(taskDiscussion => taskDiscussion.DatePosted)
                .ToListAsync();
        }
    }
}
