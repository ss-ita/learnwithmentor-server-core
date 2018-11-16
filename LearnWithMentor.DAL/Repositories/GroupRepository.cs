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
            //return await Context.Groups.Where(g => g.GroupPlans.Select(gr => gr.Plan).Any(p => p.Id == planId)).ToListAsync();
            var plans = from g in Context.GroupPlans where g.Plan.Id == planId select g.Group;
            return await plans.ToListAsync();
        }

        public async Task<bool> AddPlanToGroupAsync(int planId, int groupId)
        {
            Plan findPlan = await Context.Plans.FirstOrDefaultAsync(plan => plan.Id == planId);
            Group findGroup = await Context.Groups.FirstOrDefaultAsync(group => group.Id == groupId);
            findGroup?.GroupPlans.Add(findPlan);
            return true;
        }

        public async Task<bool> AddUserToGroupAsync(int userId, int groupId)
        {
            User findUser = await Context.Users.FirstOrDefaultAsync(user => user.Id == userId);
            Group findGroup = await Context.Groups.FirstOrDefaultAsync(group => group.Id == groupId);
            findGroup?.Users.Add(findUser);
            return true;
        }

        public async ThreadTask.Task RemoveUserFromGroupAsync(int groupId, int userId)
        {
            Group group = await GetAsync(groupId);
            User findUser = await Context.Users.FirstOrDefaultAsync(user => user.Id == userId);
            group.Users.Remove(findUser);
        }

        public async ThreadTask.Task RemovePlanFromGroupAsync(int groupId, int planId)
        {
            Group group = await GetAsync(groupId);
            Plan findPlan = await Context.Plans.FirstOrDefaultAsync(plan => plan.Id == planId);
            group.Plans.Remove(findPlan);
        }
    }
}
