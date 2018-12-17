using System.Collections.Generic;
using System.Threading.Tasks;
using LearnWithMentorDTO;

namespace LearnWithMentorBLL.Interfaces
{
    public interface IPlanService : IDisposableService
    {
        Task<int?> AddAndGetIdAsync(PlanDTO dto);
        List<PlanDTO> Search(string[] searchString);
        Task<List<PlanDTO>> GetAll();
        List<PlanDTO> GetSomeAmount(int prevAmount, int amount);
        Task<PlanDTO> GetAsync(int id);
        Task<List<TaskDTO>> GetAllTasksAsync(int planId);
        Task<string> GetInfoAsync(int groupid, int planid);
        Task<List<int>> GetAllPlanTaskidsAsync(int planId);
        Task<List<SectionDTO>> GetTasksForPlanAsync(int planId);
        Task<bool> UpdateByIdAsync(PlanDTO plan, int id);
        Task<bool> RemovePlanByIdAsync(int planId);
        Task<bool> AddAsync(PlanDTO dto);
        Task<bool> ContainsId(int id);
        Task<bool> SetImageAsync(int id, byte[] image, string imageName);
        Task<bool> AddTaskToPlanAsync(int planId, int taskId, int? sectionId, int? priority);
        Task<ImageDTO> GetImageAsync(int id);
    }
}
