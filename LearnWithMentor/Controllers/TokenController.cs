using LearnWithMentor.DAL.Entities;
using LearnWithMentor.Models;
using LearnWithMentorDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LearnWithMentor.Controllers
{
    /// <summary>
    /// Controller for tokens.
    /// </summary>
    public class TokenController : Controller
    {
        private readonly IJwtFactory jwtFactory;
        private readonly UserManager<User> userManager;

        /// <summary>
        /// Creates instance of TokenController.
        /// </summary>
        /// <param name="userService"> Dependency injection parameter. </param>
        public TokenController(IJwtFactory jwtFactory, UserManager<User> userManager)
        {
            this.jwtFactory = jwtFactory;
            this.userManager = userManager;
        }

        /// <summary>
        /// Returns new token for user.
        /// </summary>
        /// <param name="value"> User data. </param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("api/token")]
        public async  Task<IActionResult> PostAsync([FromBody]UserLoginDTO value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var identity = await GetClaimsIdentity(value.Email, value.Password);
            if (identity == null)
            {
                return Unauthorized();
            }

            var user = await userManager.FindByEmailAsync(value.Email);
            if (user == null)
                return Unauthorized();

            var userDto = new UserIdentityDTO()
            {
                Email = user.Email,
                LastName = user.LastName,
                FirstName = user.FirstName,
                Id = user.Id,
                Role = user.Role.Name,
                EmailConfirmed = user.EmailConfirmed
            };

            string jwt = JwtManager.GenerateToken(userDto);
            return new JsonResult(jwt);
        }

        private async Task<ClaimsIdentity> GetClaimsIdentity(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
                return await Task.FromResult<ClaimsIdentity>(null);

            var userToVerify = await userManager.FindByEmailAsync(userName);

            if (userToVerify == null) return await Task.FromResult<ClaimsIdentity>(null);

            if (await userManager.CheckPasswordAsync(userToVerify, password))
            {
                return await Task.FromResult(jwtFactory.GenerateClaimsIdentity(userName));
            }

            return await Task.FromResult<ClaimsIdentity>(null);
        }

        /// <summary>
        /// Releases memory
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
