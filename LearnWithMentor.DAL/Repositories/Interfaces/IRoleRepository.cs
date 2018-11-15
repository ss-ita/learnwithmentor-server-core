using LearnWithMentor.DAL.Entities;
using System.Threading.Tasks;

namespace LearnWithMentor.DAL.Repositories.Interfaces
{
    public interface IRoleRepository : IRepository<Role>
    {
        Task<Role> Get(int id);
        Task<Role> TryGetByName(string name);
    }
}
