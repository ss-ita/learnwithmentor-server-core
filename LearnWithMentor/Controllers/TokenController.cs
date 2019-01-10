using LearnWithMentor.DAL.Entities;
using LearnWithMentor.Models;
using LearnWithMentorDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LearnWithMentor.Controllers
{
    /// <summary>
    /// Controller for tokens.
    /// </summary>
    public class TokenController : Controller
    {
        private readonly UserManager<User> userManager;

        /// <summary>
        /// Creates instance of TokenController.
        /// </summary>
        /// <param name="userService"> Dependency injection parameter. </param>
        public TokenController(UserManager<User> userManager)
        {
            this.userManager = userManager;
        }

        /// <summary>
        /// Returns new token for user.
        /// </summary>
        /// <param name="value"> User data. </param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("api/token")]
        public async  Task<IActionResult> PostAsync([FromBody]UserLoginDTO value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await userManager.FindByEmailAsync(value.Email);
            if (user == null)
                return Unauthorized();

            if (await userManager.CheckPasswordAsync(user, value.Password))
            {
                var userDto = new UserIdentityDTO()
                {
                    Email = user.Email,
                    LastName = user.LastName,
                    FirstName = user.FirstName,
                    Id = user.Id,
                    Role = user.Role.Name,
                    EmailConfirmed = user.EmailConfirmed,
                    Blocked = user.Blocked,
                    Password = user.Password
                };

                string jwt = JwtManager.GenerateToken(userDto);
                return new JsonResult(jwt);
            }
            return Unauthorized();
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
