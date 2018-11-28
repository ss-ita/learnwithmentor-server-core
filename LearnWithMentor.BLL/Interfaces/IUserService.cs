using System.Collections.Generic;
using System.Threading.Tasks;
using LearnWithMentorDTO;

namespace LearnWithMentorBLL.Interfaces
{
    public interface IUserService : IDisposableService
    {
        Task<PagedListDTO<UserDTO>> GetUsers(int pageSize, int pageNumber = 1);
        Task<UserDTO> GetAsync(int id);
        Task<UserIdentityDTO> GetByEmailAsync(string email);
        Task<List<UserDTO>> GetAllUsersAsync();
        Task<bool> BlockByIdAsync(int id);
        Task<bool> ConfirmEmailByIdAsync(int id);
        Task<bool> UpdateByIdAsync(int id, UserDTO user);
        Task<bool> AddAsync(UserRegistrationDTO userLoginDTO);
        Task<List<UserDTO>> SearchAsync(string[] str, int? roleId);
        Task<List<UserDTO>> GetUsersByRoleAsync(int roleId);
        Task<List<UserDTO>> GetUsersByStateAsync(bool state);
        Task<bool> SetImageAsync(int id, byte[] image, string imageName);
        Task<ImageDTO> GetImageAsync(int id);
        Task<bool> ContainsIdAsync(int id);
        Task<bool> UpdatePasswordAsync(int userId, string password);
        Task<PagedListDTO<UserDTO>> SearchAsync(string[] str, int pageSize, int pageNumber, int? roleId);
        Task<PagedListDTO<UserDTO>> GetUsersByRoleAsync(int roleId, int pageSize, int pageNumber);
        Task<PagedListDTO<UserDTO>> GetUsersByStateAsync(bool state, int pageSize, int pageNumber);
    }
}
