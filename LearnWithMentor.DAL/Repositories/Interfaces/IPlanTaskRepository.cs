using LearnWithMentor.DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LearnWithMentor.DAL.Repositories.Interfaces
{
    public interface IPlanTaskRepository : IRepository<PlanTask>
    {
        Task<PlanTask> Get(int id);
        Task<PlanTask> Get(int taskId, int planId);
        Task<IEnumerable<PlanTask>> GetPlanTaskListByPlanAsync(int planId);
        Task<bool> ContainsTaskInPlan(int taskId, int planId);
        Task<int[]> GetTasksIdForPlan(int planId);
        Task<int[]> GetPlansIdForTask(int taskId);
        Task<int?> GetTaskPriorityInPlanAsync(int taskId, int planId);
        Task<int?> GetTaskSectionIdInPlanAsync(int taskId, int planId);
        Task<int?> GetIdByTaskAndPlanAsync(int taskId, int planId);
    }
}
