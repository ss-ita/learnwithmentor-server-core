using System.Collections.Generic;
using System;
using LearnWithMentorDTO;
using System.Threading.Tasks;

namespace LearnWithMentorBLL.Interfaces
{
    public interface ITaskService : IDisposableService
    {
        bool CreateTask(TaskDTO taskDTO);
        Task<IEnumerable<TaskDTO>> GetAllTasksAsync();
        Task<TaskDTO> GetTaskByIdAsync(int taskId);
        Task<int?> AddAndGetIdAsync(TaskDTO taskDTO);
        Task<TaskDTO> GetTaskForPlanAsync(int taskId, int planId);
        Task<TaskDTO> GetTaskForPlanAsync(int planTaskId);
        Task<IEnumerable<TaskDTO>> GetTasksNotInPlanAsync(int planId);
        Task<UserTaskDTO> GetUserTaskByUserPlanTaskIdAsync(int userId, int planTaskId);
        Task<bool> CreateUserTaskAsync(UserTaskDTO userTaskDTO);
        Task<bool> UpdateUserTaskStatusAsync(int userTaskId, string newStatus);
        Task<bool> UpdateUserTaskResultAsync(int userTaskId, string newResult);
        Task<bool> UpdateTaskByIdAsync(int taskId,TaskDTO taskDTO);
        Task<bool> RemoveTaskByIdAsync(int taskId);
        Task<bool> UpdateProposeEndDateAsync(int userTaskId, DateTime proposeEndDate);
        Task<List<UserTaskDTO>> GetTaskStatesForUserAsync(int[] planTaskIds, int userId);
        Task<IEnumerable<TaskDTO>> SearchAsync(string[] str, int planId);
        Task<List<TaskDTO>> SearchAsync(string[] keys);
        Task<StatisticsDTO> GetUserStatisticsAsync(int userId);
        Task<UserDTO> GetUserByUserTaskId(int userTaskId);
        Task<UserDTO> GetMentorByUserTaskId(int userTaskId);
        Task<PagedListDTO<TaskDTO>> GetTasks(int pageSize, int pageNumber = 1);
        Task<bool> DeleteProposeEndDateAsync(int userTaskId);
        Task<bool> SetNewEndDateAsync(int userTaskId);
        Task<bool> CheckUserTaskOwnerAsync(int userTaskId, int userId);
        Task<bool> AddTaskToPlanAsync(TaskDTO taskDTO, int? selectedPriority, int? selectedSection);
    }
}
