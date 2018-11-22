using LearnWithMentor.Models;
using LearnWithMentorBLL.Interfaces;
using LearnWithMentorDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LearnWithMentor.Controllers
{
    /// <summary>
    /// Controller for tokens.
    /// </summary>
    public class TokenController : Controller
    {
        private readonly IUserService userService;

        /// <summary>
        /// Creates instance of TokenController.
        /// </summary>
        /// <param name="userService"> Dependency injection parameter. </param>
        public TokenController(IUserService userService)
        {
            this.userService = userService;
        }

        /// <summary>
        /// Returns new token for user.
        /// </summary>
        /// <param name="value"> User data. </param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("api/token")]
        public async  Task<IActionResult> PostAsync([FromBody]UserLoginDto value)
        {
            UserIdentityDto user = null;
            user = await CheckUserAsync(value.Email);
            bool UserCheck =  BCrypt.Net.BCrypt.Verify(value.Password, user.Password);
            if ((ModelState.IsValid) && (UserCheck))
            {
                if (user.Blocked.HasValue && user.Blocked.Value)
                {
                    return Unauthorized();
                }

                string token = JwtManager.GenerateToken(user);
                var response = new JsonResult(token);

                return response;
            }

            return Unauthorized();
        }

        /// <summary>
        /// Checks if user with provided email exists
        /// </summary>
        /// <param name="email"></param>
        /// <returns>UserIdentityDto</returns>
        public async Task<UserIdentityDto> CheckUserAsync(string email)
        {
            return await userService.GetByEmailAsync(email);
        }

        /// <summary>
        /// Releases memory
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            userService.Dispose();
            base.Dispose(disposing);
        }
    }
}
