using LearnWithMentor.DAL.Entities;
using LearnWithMentor.DAL.Repositories.Interfaces;

namespace LearnWithMentor.DAL.Repositories
{
    public class UserRoleViewRepository : BaseRepository<UserRole>, IUserRoleViewRepository
    {
        public UserRoleViewRepository(LearnWithMentorContext context) : base(context)
        {
        }
    }
}
