using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LearnWithMentor.DAL.Entities;
using LearnWithMentor.DAL.Repositories.Interfaces;

namespace LearnWithMentor.DAL.Repositories
{
    public class TaskRepository : BaseRepository<StudentTask>, ITaskRepository
    {
        public TaskRepository(LearnWithMentorContext context) : base(context)
        {
        }

        public Task<StudentTask> GetAsync(int id)
        {
            return Context.Tasks.FirstOrDefaultAsync(task => task.Id == id);

        }

        public async Task<bool> IsRemovableAsync(int id)
        {
            return await Context.PlanTasks.AnyAsync(planTask => planTask.Task_Id == id);

        }

        public StudentTask AddAndReturnElement(StudentTask task)
        {
            Context.Tasks.Add(task);
            return task;
        }

        public async Task<bool> DeleteTaskAsync(int taskId)
        {
            IEnumerable<PlanTask> planTasks = Context.PlanTasks.Where(planTask => planTask.Task_Id == taskId);
            
            foreach(var comments in planTasks.Select(p => p.Comments))
            {
                foreach(var comment in comments)
                {
                    Context.Comments.Remove(comment);
                }
            }
            await Context.SaveChangesAsync();

            foreach (var userTasks in planTasks.Select(p => p.UserTasks))
            {
                foreach(var messages in userTasks.Select(u => u.Messages))
                {
                    foreach(var message in messages)
                    {
                        Context.Messages.Remove(message);
                    }
                }
                await Context.SaveChangesAsync();
                Context.UserTasks.RemoveRange(userTasks);
            }
            await Context.SaveChangesAsync();

            Context.PlanTasks.RemoveRange(planTasks);
            await Context.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<StudentTask>> SearchAsync(string[] str, int planId)
        {
            bool checkPlanExisting = await Context.Plans.AnyAsync(plan => plan.Id == planId);
            if (!checkPlanExisting)
            {
                return null;
            }
            List<StudentTask> result = new List<StudentTask>();
            foreach (var word in str)
            {
                IEnumerable<StudentTask> tasks = Context.PlanTasks.Where(plan => plan.Plan_Id == planId)
                                             .Select(planTask => planTask.Tasks)
                                             .Where(task => task.Name.Contains(word));
                foreach (var task in tasks)
                {
                    if (!result.Contains(task))
                    {
                        result.Add(task);
                    }
                }
            }
            return result;
        }

        public async Task<IEnumerable<StudentTask>> SearchAsync(string[] str)
        {
            List<StudentTask> result = new List<StudentTask>();
            foreach (var word in str)
            {
                IEnumerable<StudentTask> tasks = await Context.Tasks.Where(task => task.Name.Contains(word)).ToListAsync();
                foreach (var task in tasks)
                {
                    if (!result.Contains(task))
                    {
                        result.Add(task);
                    }
                }
            }
            return result;
        }

        public async Task<IEnumerable<StudentTask>> GetTasksNotInPlanAsync(int planId)
        {
            var usedTasks = await Context.PlanTasks.Where(planTask => planTask.Plan_Id == planId).Select(planTask => planTask.Task_Id).ToListAsync();
            return await Context.Tasks.Where(tasks => !usedTasks.Contains(tasks.Id)).ToListAsync();
        }
    }
}
