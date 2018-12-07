using LearnWithMentor.DAL.Entities;
using System.Threading.Tasks;

namespace LearnWithMentor.DAL.Repositories.Interfaces
{
    public interface IUserTaskRepository : IRepository<UserTask>
    {
        Task<UserTask> GetAsync(int id);
        Task<UserTask> GetByPlanTaskForUserAsync(int planTaskId, int userId);
        Task<int> GetNumberOfTasksByStateAsync(int userId, string state);
        Task<User> GetUserAsync(int userTaskId);
        Task<User> GetMentorAsync(int userTaskId);
    }
}
