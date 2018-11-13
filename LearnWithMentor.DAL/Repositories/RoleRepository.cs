using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using LearnWithMentor.DAL.Entities;
using LearnWithMentor.DAL.Repositories.Interfaces;

namespace LearnWithMentor.DAL.Repositories
{
    public class RoleRepository : BaseRepository<Role>, IRoleRepository
    {
        public RoleRepository(LearnWithMentorContext context) : base(context)
        {
        }

        public Task<Role> Get(int id)
        {
            return Context.Roles.FirstOrDefaultAsync(r => r.Id == id);
        }

        public Task<Role> TryGetByName(string name)
        {
            return Context.Roles.FirstOrDefaultAsync(r => r.Name == name);
        }
    }
}
