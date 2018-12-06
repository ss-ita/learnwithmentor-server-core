using System.Collections.Generic;
using LearnWithMentorBLL.Interfaces;
using LearnWithMentorDTO;
using LearnWithMentor.DAL.Entities;
using System;
using System.Linq;
using LearnWithMentor.DAL.UnitOfWork;
using System.Threading.Tasks;

namespace LearnWithMentorBLL.Services
{
    public class UserService : BaseService, IUserService
    {
        public UserService(IUnitOfWork db) : base(db)
        {
        }

        public async Task<UserDTO> GetAsync(int id)
        {
            User user = await db.Users.GetAsync(id);
            if (user == null)
            {
                return null;
            }
            return await UserToUserDTOAsync(user);
        }

        public async Task<UserIdentityDTO> GetByEmailAsync(string email)
        {
            User user = await db.Users.GetByEmailAsync(email);
            if (user == null)
            {
                return null;
            }
            return new UserIdentityDTO(user.Email, user.Password, user.Id,
                user.FirstName,
                user.LastName,
                user.Role.Name,
                user.Blocked,
                user.EmailConfirmed);
        }

        public async Task<List<UserDTO>> GetAllUsersAsync()
        {
            IEnumerable<User> users = await db.Users.GetAll();
            if (users == null)
            {
                return null;
            }
            var dtos = new List<UserDTO>();
            foreach (var user in users)
            {
                dtos.Add(await UserToUserDTOAsync(user));
            }
            return dtos;
        }

        public async Task<PagedListDTO<UserDTO>> GetUsers(int pageSize, int pageNumber = 0)
        {
            //var query =  await db.Users.GetAll().AsQueryable();
            //query = query.OrderBy(x => x.Id);
            var queryLan =  await db.Users.GetAll();
            var query = queryLan.AsQueryable();
            query = query.OrderBy(x => x.Id);

            return await PagedList<User, UserDTO>.GetDTO(query, pageNumber, pageSize, UserToUserDTOAsync);
        }

        public async Task<bool> BlockByIdAsync(int id)
        {
            User item = await db.Users.GetAsync(id);
            if (item == null)
            {
                return false;
            }
            item.Blocked = true;
            await db.Users.UpdateAsync(item);
            db.Save();
            return true;
        }

        public async Task<bool> UpdateByIdAsync(int id, UserDTO user)
        {
            var modified = false;
            User item = await db.Users.GetAsync(id);
            if (item != null)
            {
                if (user.FirstName != null)
                {
                    item.FirstName = user.FirstName;
                    modified = true;
                }
                if (user.LastName != null)
                {
                    item.LastName = user.LastName;
                    modified = true;
                }
                if (user.Blocked != null)
                {
                    item.Blocked = user.Blocked.Value;
                    modified = true;
                }
                var updatedRole = await db.Roles.TryGetByName(user.Role);
                if (updatedRole != null)
                {
                    item.Role_Id = updatedRole.Id;
                    modified = true;
                }
                await db.Users.UpdateAsync(item);
                db.Save();
            }
            return modified;
        }

        public async Task<bool> ConfirmEmailByIdAsync(int id)
        {
            User user = await db.Users.GetAsync(id);
            if (user != null)
            {
                user.EmailConfirmed = true;
                await db.Users.UpdateAsync(user);
                db.Save();
                return true;
            }
            return false;
        }

        public async Task<List<UserDTO>> SearchAsync(string[] str, int? roleId)
        {
            IEnumerable<User> users = await  db.Users.SearchAsync(str, roleId);
            var dtos = new List<UserDTO>();
            foreach (var user in users)
            {
                dtos.Add( await UserToUserDTOAsync(user));
            }
            return dtos;
        }

        public async Task<PagedListDTO<UserDTO>> SearchAsync(string[] str, int pageSize, int pageNumber, int? roleId)
        {
            var query = (await db.Users.SearchAsync(str, roleId)).AsQueryable();
            query = query.OrderBy(x => x.Id);
            return await PagedList<User, UserDTO>.GetDTO(query, pageNumber, pageSize, UserToUserDTOAsync);
        }

        public async Task<List<UserDTO>> GetUsersByRoleAsync(int roleId)
        {
            IEnumerable<User> users = await db.Users.GetUsersByRoleAsync(roleId);
            if (users == null)
            {
                return null;
            }
            var dtos = new List<UserDTO>();
            foreach (var user in users)
            {
                dtos.Add( await UserToUserDTOAsync(user));
            }
            return dtos;
        }

        public async Task<PagedListDTO<UserDTO>> GetUsersByRoleAsync(int roleId, int pageSize, int pageNumber)
        {
            var query = await db.Users.GetUsersByRoleAsync(roleId);
            var queryAwait = query.AsQueryable();

            queryAwait = queryAwait.OrderBy(x => x.Id);
            return await PagedList<User, UserDTO>.GetDTO(queryAwait, pageNumber, pageSize, UserToUserDTOAsync);
        }

        public async Task<bool> SetImageAsync(int id, byte[] image, string imageName)
        {
            User userToUpdate = await db.Users.GetAsync(id);
            if (userToUpdate == null)
            {
                return false;
            }
            var converted = Convert.ToBase64String(image);
            userToUpdate.Image = converted;
            userToUpdate.Image_Name = imageName;
            db.Save();
            return true;
        }

        public async Task<ImageDTO> GetImageAsync(int id)
        {
            User userToGetImage = await db.Users.GetAsync(id);
            if (userToGetImage?.Image == null || userToGetImage.Image_Name == null)
            {
                return null;
            }
            return new ImageDTO()
            {
                Name = userToGetImage.Image_Name,
                Base64Data = userToGetImage.Image
            };
        }

        public async Task<bool> ContainsIdAsync(int id)
        {
            return await db.Users.ContainsIdAsync(id);
        }

        public async Task<List<UserDTO>> GetUsersByStateAsync(bool state)
        {
            IEnumerable<User> users = await db.Users.GetUsersByStateAsync(state);
            if (users == null)
            {
                return null;
            }
            var dtos = new List<UserDTO>();
            foreach (var user in users)
            {
                  dtos.Add( await UserToUserDTOAsync(user));
            }
            return dtos;
        }

        public async Task<PagedListDTO<UserDTO>> GetUsersByStateAsync(bool state, int pageSize, int pageNumber)
        {
            var GetQuery = await db.Users.GetUsersByStateAsync(state);
            var query =  GetQuery.AsQueryable();
            query = query.OrderBy(x => x.Id);
            return await PagedList<User, UserDTO>.GetDTO(query, pageNumber, pageSize, UserToUserDTOAsync);
        }

        private async Task<UserDTO> UserToUserDTOAsync(User user)
        {
            return new UserDTO(user.Id,
                               user.FirstName,
                               user.LastName,
                               user.Email,
                               user.Role.Name,
                               user.Blocked,
                               user.EmailConfirmed);
        }
    }
}
