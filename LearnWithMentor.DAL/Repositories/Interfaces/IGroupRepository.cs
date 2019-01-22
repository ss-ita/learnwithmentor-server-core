using System.Collections.Generic;
using System.Threading.Tasks;
using LearnWithMentor.DAL.Entities;
using Task = System.Threading.Tasks.Task;

namespace LearnWithMentor.DAL.Repositories.Interfaces
{
    public interface IGroupRepository : IRepository<Group>
    {
        Task<Group> GetAsync(int id);
        Task<int> CountAsync();
        Task<bool> GroupNameExistsAsync(string groupName);
        Task<IEnumerable<Group>> GetGroupsByMentorAsync(int mentorId);
        Task<bool> AddUserToGroupAsync(int userId, int groupId);
        Task<bool> AddPlanToGroupAsync(int planId, int groupId);
        Task RemoveUserFromGroupAsync(int groupId, int userId);
        Task RemovePlanFromGroupAsync(int groupId, int planId);
        Task<IEnumerable<Group>> GetStudentGroupsAsync(int studentId);
        Task<IEnumerable<Group>> GetGroupsByPlanAsync(int planId);
        Task<Group> AddAndReturnElement(Group group);
    }
}
