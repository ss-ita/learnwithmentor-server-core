using System.Collections.Generic;
using LearnWithMentorDTO;
using System.Threading.Tasks;

namespace LearnWithMentorBLL.Interfaces
{
    public interface IRoleService : IDisposableService
    {
        Task<RoleDTO> GetAsync(int id);
        Task<List<RoleDTO>> GetAllRoles();
        Task<RoleDTO> GetByNameAsync(string name);
    }
}