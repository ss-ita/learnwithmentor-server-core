using System.Linq;
using LearnWithMentor.DAL.Entities;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using LearnWithMentor.DAL.Repositories.Interfaces;
using ThreadTask = System.Threading.Tasks;

namespace LearnWithMentor.DAL.Repositories
{
    public class GroupRepository : BaseRepository<Group>, IGroupRepository
    {
        public GroupRepository(LearnWithMentorContext context) : base(context)
        {
        }

        public async Task<Group> GetAsync(int id)
        {
            return await Context.Groups.FirstOrDefaultAsync(group => group.Id == id);
        }

        public async Task<bool> GroupNameExistsAsync(string groupName)
        {
            return await Context.Groups.AnyAsync(g => g.Name.Equals(groupName));
        }

        public async Task<int> CountAsync()
        {
            return await Context.Groups.CountAsync();
        }

        public async Task<IEnumerable<Group>> GetGroupsByMentorAsync(int mentorId)
        {
            return await Context.Groups.Where(group => group.Mentor_Id == mentorId).ToListAsync();
        }

        public async Task<IEnumerable<Group>> GetStudentGroupsAsync(int studentId)
        {
            User findStudent = await Context.Users.FirstOrDefaultAsync(u => u.Id == studentId);
            return findStudent?.UserGroups.Select(g => g.Group);
        }

        public async Task<IEnumerable<Group>> GetGroupsByPlanAsync(int planId)
        {
            var plans = from g in Context.GroupPlans where g.Plan.Id == planId select g.Group;
            return await plans.ToListAsync();
        }

        public async Task<bool> AddPlanToGroupAsync(int planId, int groupId)
        {
            Plan plan = await Context.Plans.FirstOrDefaultAsync(p => p.Id == planId);
            Group group = await Context.Groups.FirstOrDefaultAsync(g => g.Id == groupId);

            if (plan == null || group == null)
            {
                return false;
            }

            GroupPlan groupPlan = new GroupPlan()
            {
                GroupId = groupId,
                PlanId = planId
            };

            await Context.GroupPlans.AddAsync(groupPlan);
            return true;
        }

        public async Task<bool> AddUserToGroupAsync(int userId, int groupId)
        {
            UserGroup findUser = await Context.UserGroups.FirstOrDefaultAsync(user => user.User.Id == userId);
            Group findGroup = await Context.Groups.FirstOrDefaultAsync(group => group.Id == groupId);
            findGroup?.UserGroups.Add(findUser);
            return true;
        }

        public async ThreadTask.Task RemoveUserFromGroupAsync(int groupId, int userId)
        {
            Group group = await GetAsync(groupId);
            UserGroup findUser = await Context.UserGroups.FirstOrDefaultAsync(user => user.User.Id == userId);
            group.UserGroups.Remove(findUser);
        }

        public async ThreadTask.Task RemovePlanFromGroupAsync(int groupId, int planId)
        {
            Group group = await GetAsync(groupId);
            GroupPlan findPlan = await Context.GroupPlans.FirstOrDefaultAsync(plan => plan.Plan.Id == planId);
            group.GroupPlans.Remove(findPlan);
        }
    }
}
