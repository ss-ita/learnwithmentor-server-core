using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LearnWithMentor.DAL.Entities;
using LearnWithMentor.DAL.Repositories.Interfaces;

namespace LearnWithMentor.DAL.Repositories
{
    public class UserTaskRepository : BaseRepository<UserTask>, IUserTaskRepository
    {
        public UserTaskRepository(LearnWithMentorContext context) : base(context)
        {
        }

        public Task<UserTask> GetAsync(int id)
        {
            return Context.UserTasks.FirstOrDefaultAsync(task => task.Id == id);
        }

        public Task<int> GetNumberOfTasksByStateAsync(int userId, string state)
        {
            return Context.UserTasks.Where(userTask => userTask.User_Id == userId).CountAsync(userTask => userTask.State == state);
        }

        public Task<UserTask> GetByPlanTaskForUserAsync(int planTaskId, int userId)
        {
            return Context.UserTasks.FirstOrDefaultAsync(userTask => userTask.User_Id == userId && userTask.PlanTask_Id == planTaskId);
        }

        public Task<User> GetUserAsync(int userTaskId)
        {
            return Context.UserTasks.Where(userTask => userTask.Id == userTaskId).Select(userTask => userTask.User).FirstOrDefaultAsync();
        }

        public Task<User> GetMentorAsync(int userTaskId)
        {
            return Context.UserTasks.Where(userTask => userTask.Id == userTaskId).Select(userTask => userTask.Mentor).FirstOrDefaultAsync();
        }
    }
}
