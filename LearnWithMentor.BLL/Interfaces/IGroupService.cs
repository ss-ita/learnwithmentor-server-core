using System.Collections.Generic;
using System.Threading.Tasks;
using LearnWithMentorDTO;

namespace LearnWithMentorBLL.Interfaces
{
    public interface IGroupService : IDisposableService
    {
        Task<GroupDTO> GetGroupByIdAsync(int id);
        Task<int> GroupsCountAsync();
        Task<int?> GetMentorIdByGroupAsync(int groupId);
        Task<IEnumerable<GroupDTO>> GetGroupsByMentorAsync(int mentorId);
        Task<IEnumerable<UserIdentityDTO>> GetUsersAsync(int groupId);
        Task<IEnumerable<UserWithImageDTO>> GetUsersWithImageAsync(int groupId);
        Task<IEnumerable<PlanDTO>> GetPlansAsync(int groupId);
        Task<bool> AddGroupAsync(GroupDTO group);        
        Task<bool> AddUsersToGroupAsync(int[] usersId, int groupId);
        Task<bool> AddPlansToGroupAsync(int[] plansId, int groupId);
        Task<IEnumerable<UserIdentityDTO>> GetUsersNotInGroupAsync(int groupId);
        Task<IEnumerable<UserIdentityDTO>> SearchUserNotInGroupAsync(string[] searchCases, int groupId);
        Task<IEnumerable<PlanDTO>> GetPlansNotUsedInGroupAsync(int groupId);
        Task<IEnumerable<PlanDTO>> SearchPlansNotUsedInGroupAsync(string[] searchCases, int groupId);
        Task<bool> RemoveUserFromGroupAsync(int groupId, int userIdToRemove);
        Task<bool> RemovePlanFromGroupAsync(int groupId, int planIdToRemove);
        Task<IEnumerable<GroupDTO>> GetUserGroupsAsync(int userId);
        Task<List<int>> GetUserGroupsIdAsync(int userId);
        Task<int?> AddAndGetIdAsync(GroupDTO dto);
    }
}
