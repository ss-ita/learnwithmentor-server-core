using LearnWithMentor.DAL.Entities;
using LearnWithMentor.DAL.Repositories.Interfaces;

namespace LearnWithMentor.DAL.Repositories
{
    public class GroupPlanRepository : BaseRepository<GroupPlan>, IGroupPlanRepository
    {
        public GroupPlanRepository(LearnWithMentorContext context) : base(context) { }
    }
}
