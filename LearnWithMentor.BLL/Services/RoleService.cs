using System.Collections.Generic;
using LearnWithMentorBLL.Interfaces;
using LearnWithMentorDTO;
using LearnWithMentor.DAL.UnitOfWork;
using System.Threading.Tasks;

namespace LearnWithMentorBLL.Services
{
    public class RoleService : BaseService, IRoleService
    {
        public RoleService(IUnitOfWork db) : base(db)
        {
        }
        public async Task<RoleDTO> GetAsync(int id)
        {
            var role = await db.Roles.Get(id);
            return role == null ? null :
                new RoleDTO(role.Id, role.Name);
        }
        public async Task<List<RoleDTO>> GetAllRoles()
        {
            var roles = await db.Roles.GetAll();
            if (roles == null)
            {
                return null;
            }
            var dtos = new List<RoleDTO>();
            foreach (var role in roles)
            {
                dtos.Add(new RoleDTO(role.Id, role.Name));
            }
            return dtos;
        }
        public async Task<RoleDTO> GetByNameAsync(string name)
        {
            var role = await db.Roles.TryGetByName(name);
            if (role == null)
            {
                return null;
            }
            return new RoleDTO(role.Id, role.Name);
        }
    }
}
