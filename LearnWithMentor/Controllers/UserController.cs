using LearnWithMentorDTO;
using System.Data;
using LearnWithMentorBLL.Interfaces;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.WebSockets.Internal;
using System.IO;
using System.Linq;
using LearnWithMentor.Services;
using LearnWithMentor.Models;
using Microsoft.AspNetCore.Http;

namespace LearnWithMentor.Controllers
{
    /// <summary>
    /// Controller for system users.
    /// </summary>
    [Authorize]
    public class UserController : Controller
    {
        private readonly IUserService userService;
        private readonly IRoleService roleService;
        private readonly ITaskService taskService;
        private readonly IUserIdentityService userIdentityService;
		private readonly IHttpContextAccessor _accessor;
		/// <summary>
		/// Creates an instance of UserController.
		/// </summary>
		public UserController(IUserService userService, IRoleService roleService, ITaskService taskService, IUserIdentityService userIdentityService, IHttpContextAccessor accessor)
        {
            this.userService = userService;
            this.roleService = roleService;
            this.taskService = taskService;
            this.userIdentityService = userIdentityService;
			this._accessor = accessor;
        }
		/// <summary>
		/// Returns all users of the system.
		/// </summary>
		[Authorize(Roles = "Admin, Mentor")]
        [HttpGet]
        [Route("api/user")]
        public async Task<ActionResult> GetAsync()
        {
            List<UserDto> users = await userService.GetAllUsersAsync();
            if (users.Count != 0)
            {
				return Ok(users);
            }
			return NoContent();
		}
        /// <summary>
        /// Returns one page of users
        /// </summary>
        [Authorize(Roles = "Admin, Mentor")]
        [HttpGet]
        [Route("api/user")]
        public async Task<ActionResult> GetAsync([FromQuery]int pageSize, [FromQuery]int pageNumber)
        {
            try
            {
                PagedListDto<UserDto> users = await userService.GetUsers(pageSize, pageNumber);
				return Ok(users);
            }
            catch (Exception e)
            {
				//tracer.Error(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, e);
				return StatusCode(500);
			}
        }

        /// <summary>
        /// Returns all users with specified role.
        /// </summary>
        /// <param name="roleId"> Id of the role. </param>

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("api/user/inrole/{roleId}")]
        public async Task<ActionResult> GetUsersbyRoleAsync(int roleId)
        {
			
            if (roleId != Constants.Roles.BlockedIndex)
            {
                var role = await roleService.GetAsync(roleId);
                if (role == null)
                {
					return NoContent();
                }
            }
            List<UserDto> users = await userService.GetUsersByRoleAsync(roleId);
            if (users.Count == 0)
            {
				return NoContent();
            }
			return Ok(users);
        }
        /// <summary>
        /// Returns one page of users with specified role.
        /// </summary>
        /// <param name="roleId"> Id of the role. </param>
        /// <param name="pageSize"> Ammount of users that you want to see on one page</param>
        /// <param name="pageNumber"> Page number</param>

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("api/user/inrole/{roleId}")]
        public async Task<ActionResult> GetUsersbyRoleAsync(int roleId, [FromQuery]int pageSize, [FromQuery]int pageNumber)
        {
            if (roleId != Constants.Roles.BlockedIndex)
            {
                var role = await roleService.GetAsync(roleId);
                if (role == null)
                {
					return NoContent();
                }
            }
            PagedListDto<UserDto> users = await userService.GetUsersByRoleAsync(roleId, pageSize, pageNumber);
			return Ok(users);
        }

        /// <summary>
        /// Returns all blocked/unblocked users.
        /// </summary>
        /// <param name="state"> Specifies value of Blocked property of user. </param>

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("api/user/instate/{state}")]
        public async Task<ActionResult> GetUsersbyStateAsync(bool state)
        {
            List<UserDto> users = await userService.GetUsersByStateAsync(state);
            if (users.Count == 0)
            {
				return NoContent();
            }
			return Ok(users);
        }

        /// <summary>
        /// Returns one page of blocked/unblocked users.
        /// </summary>
        /// <param name="state"> Specifies value of Blocked property of user. </param>
        /// <param name="pageSize"> Ammount of users that you want to see on one page</param>
        /// <param name="pageNumber"> Page number</param>

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("api/user/instate/{state}")]
        public async Task<ActionResult> GetUsersbyStateAsync(bool state, [FromQuery]int pageSize, [FromQuery]int pageNumber)
        {
            PagedListDto<UserDto> users = await userService.GetUsersByStateAsync(state, pageSize, pageNumber);
			return Ok(users);
        }

        /// <summary>
        /// Returns specific user by id if exists or get id from token.
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>

        [HttpGet]
        [Route("api/user/profile/{id?}")]
        public async Task<ActionResult> GetSingleAsync(int id = 0 )
        {
            if (id == 0)
            {
                id = userIdentityService.GetUserId();
            }

            UserDto user = await userService.GetAsync(id);
            if (user != null)
            {
				return Ok(user);
            }
			return NoContent();
        }

        /// <summary>
        /// Creates new user.
        /// </summary>
        /// <param name="value"> New user. </param>
        [AllowAnonymous]
        [HttpPost]
        [Route("api/user")]
        public async Task<ActionResult> PostAsync([FromBody]UserRegistrationDto value)
        {
            if (!ModelState.IsValid)
            {
				return BadRequest(ModelState);
            }
            try
            {
                var success = await userService.AddAsync(value);
                if (success)
                {
                    var okMessage = $"Succesfully created user: {value.Email}.";
					return Ok(okMessage);
                }
            }
            catch (Exception e)
            {
				//tracer.Error(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, e);
				return StatusCode(500);
            }
            const string message = "Incorrect request syntax.";
			//tracer.Warn(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, message);
			return BadRequest(message);
        }

		/// <summary>
		/// Verifies reset password token.
		/// </summary>
		/// <param name="token"> Users token. </param>
		/// 


		//FIX IT!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
		//    [AllowAnonymous]
		//    [HttpGet]
		//    [Route("api/user/verify-token")]
		//    public async Task<ActionResult> VerifyTokenAsync(string token)
		//    {
		//        try
		//        {
		//            //if (JwtAuthenticationAttribute.ValidateToken(token, out string userEmail))
		//            {
		//                UserIdentityDto user = await userService.GetByEmailAsync(userEmail);
		//                if (user == null)
		//                {
		//		//return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "User not found");
		//		return BadRequest("User not found");
		//                }
		//	//return Request.CreateResponse(HttpStatusCode.OK, user.Id);
		//	return Ok(user.Id);
		//            }
		////return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Token no longer valid");
		//return BadRequest("Token no longer valid");
		//        }
		//        catch (Exception e)
		//        {
		////return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
		//return StatusCode(500);
		//        }
		//    }

		/// <summary>
		/// Confirms user email by token.
		/// </summary>
		/// <param name="token"> Users token. </param>
		//    [AllowAnonymous]
		//    [HttpGet]
		//    [Route("api/user/confirm-email")]
		//    public async Task<ActionResult> ConfirmEmailAsync(string token)
		//    {
		//        try
		//        {
		//            if (JwtAuthenticationAttribute.ValidateToken(token, out string userEmail))
		//            {
		//                UserIdentityDto user = await userService.GetByEmailAsync(userEmail);
		//                if (user == null)
		//                {
		//		//return Request.CreateErrorResponse(HttpStatusCode.NoContent, "User not found");
		//		return BadRequest("User not found");
		//                }
		//                if ( await userService.ConfirmEmailByIdAsync(user.Id))
		//                {
		//		//return Request.CreateResponse(HttpStatusCode.OK, "Email confirmed");
		//		return Ok("Email confirmed");
		//                }
		//	//return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Confirmation error");
		//	return BadRequest("Confirmation error");
		//            }
		////return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Token no longer valid");
		//return BadRequest("Token no longer valid");
		//        }
		//        catch (Exception e)
		//        {
		////return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
		//return StatusCode(500);
		//        }
		//    }

		/// <summary>
		/// Sends email with link for user's password reset.
		/// </summary>
		/// <param name="emailModel"> User's email. </param>
		/// <param name="resetPasswordLink"> Link on the reset page. </param>
		[AllowAnonymous]
        [HttpPost]
        [Route("api/user/password-reset")]
        public async Task<ActionResult> SendPasswordResetLinkAsync([FromBody] EmailDto emailModel, string resetPasswordLink)
        {
            try
            {
                if (resetPasswordLink == null)
                {
					var message = "Password reset link not found";
					return BadRequest(message);
                }
                if (ModelState.IsValid)
                {
                    UserIdentityDto user = await userService.GetByEmailAsync(emailModel.Email);
                    if (user == null)
                    {
						return NoContent();
                    }
                    if (!user.EmailConfirmed)
                    {
						return Forbid("Not allowed because email not confirmed");
                    }
					string token = JwtManager.GenerateToken(user, 0, 1);
					await EmailService.SendPasswordResetEmail(user.Email, token, resetPasswordLink);
					var message = "Token successfully sent";
					return Ok(message);
                }
				var mes = "Email is not valid";
				return BadRequest(mes);
            }
            catch (Exception e)
            {
				return StatusCode(500);
            }
        }

        /// <summary>
        /// Sends email with link for user's email confirmation.
        /// </summary>
        /// <param name="emailModel"> User's email. </param>
        /// <param name="emailConfirmLink"> Link on the email confirm page. </param>
        [AllowAnonymous]
        [HttpPost]
        [Route("api/user/confirm-email")]
        public async Task<ActionResult> SendEmailConfirmLinkAsync([FromBody] EmailDto emailModel, string emailConfirmLink)
        {
            try
            {
                if (emailConfirmLink == null)
                {
					var message = "Email confirmation link not found";
					return BadRequest(message);
                }
                if (ModelState.IsValid)
                {
                    UserIdentityDto user = await userService.GetByEmailAsync(emailModel.Email);
                    if (user == null)
                    {
						return NoContent();
                    }
                    if (user.EmailConfirmed)
                    {
						var message = "Email already confirmed";
						return Forbid(message);
                    }
					string token = JwtManager.GenerateToken(user, 0, 1);
					await EmailService.SendConfirmPasswordEmail(user.Email, token, emailConfirmLink);
					var mes = "Token successfully sent";
					return Ok(mes);
                }
				var mess = "Email is not valid";
				return BadRequest(mess);
            }
            catch (Exception e)
            {
				return StatusCode(500);
            }
        }

        /// <summary>
        /// Returns statistics dto with number of tasks in different states for one user.
        /// </summary>

        [HttpGet]
        [Route("api/user/statistics")]
        public async Task<ActionResult> GetStatisticsAsync()
        {
            var id = userIdentityService.GetUserId();
            StatisticsDto statistics = await taskService.GetUserStatisticsAsync(id);
            if (statistics == null)
            {
				return NoContent();
            }
			return Ok(statistics);
        }

		/// <summary>
		/// Sets user image to database
		/// </summary>
		/// <param name="id"> Id of the user. </param>
		/// 


		[HttpPost]
		[Route("api/user/{id}/image")]
		public async Task<ActionResult> PostImageAsync(int id)
		{
			if (!(await userService.ContainsIdAsync(id)))
			{
				return NoContent();
			}
			if (_accessor.HttpContext.Request.Form.Files.Count != 1)
			{
				const string errorMessage = "Only one image can be sent.";
				return BadRequest(errorMessage);
			}
			try
			{
				var postedFile = HttpContext.Request.Form.Files[0];
				if (postedFile.Length > 0)
				{
					var allowedFileExtensions = new List<string>(Constants.ImageRestrictions.Extensions);
					string extension = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.')).ToLower();
					if (!allowedFileExtensions.Contains(extension))
					{
						const string errorMessage = "Types allowed only .jpeg .jpg .png";
						return BadRequest(errorMessage);
					}
					const int maxContentLength = Constants.ImageRestrictions.MaxSize;
					if (postedFile.Length > maxContentLength)
					{
						const string errorMessage = "Please Upload a file upto 1 mb.";
						return BadRequest(errorMessage);
					}
					byte[] imageData;
					using (var binaryReader = new BinaryReader(postedFile.OpenReadStream()))
					{
						imageData = binaryReader.ReadBytes(Convert.ToInt32(postedFile.Length));
					}
				await userService.SetImageAsync(id, imageData, postedFile.FileName);
				const string okMessage = "Successfully created image.";
					return Ok(okMessage);
				}
				const string emptyImageMessage = "Empty image";
				return NotFound(emptyImageMessage);
			}
			catch (Exception e)
			{
				return BadRequest();
			}
		}

		/// <summary>
		/// Reyurns image for specific user
		/// </summary>
		/// <param name="id"> Id of the user. </param>
		

        [HttpGet]
        [Route("api/user/{id}/image")]
        public async Task<ActionResult> GetImageAsync(int id)
        {
            try
            {
                if (!(await userService.ContainsIdAsync(id)))
                {
					return NoContent();
                }
                ImageDto dto = await userService.GetImageAsync(id);
                if (dto == null)
                {
					return NoContent();
                }
				return Ok(dto);
            }
            catch (Exception e)
            {
				return BadRequest(e);
            }
        }

        /// <summary>
        /// Updates user by id.
        /// </summary>
        /// <param name="id"> Id of the user. </param>
        /// <param name="value"> New values. </param>
        

        [HttpPut]
        [Route("api/user/{id}")]
        public async Task<ActionResult> PutAsync(int id, [FromBody]UserDto value)
        {
            try
            {
                bool success = await userService.UpdateByIdAsync(id, value);
				if (success)
				{
					var okMessage = $"Succesfully updated user id: {id}.";
					return Ok(okMessage);
					//tracer.Info(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, okMessage);
				}
            }
            catch (Exception e)
            {
				return StatusCode(500);
                //tracer.Error(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, e);
            }
            const string message = "Incorrect request syntax or user does not exist.";
			return BadRequest(message);
            //tracer.Warn(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, message);
        }
		

        [HttpPut]
        [Route("api/user/update-multiple")]
        public async Task<ActionResult> UpdateUsersAsync([FromBody]UserDto[] value)
        {
            try
            {
                bool success = true;
                foreach (var item in value)
                {
                    var result = await userService.UpdateByIdAsync(item.Id, item);
                    success = success && result;
                }
                if (success)
                {
                    var okMessage = "Succesfully updated users.";
					return Ok(okMessage);
                    //tracer.Info(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, okMessage);
                }
            }
            catch (Exception e)
            {
				return StatusCode(500);
                //tracer.Error(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, e);
            }
            const string message = "Incorrect request syntax or users do not exist.";
			return BadRequest(message);
            //tracer.Warn(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, message);
        }

        /// <summary>
        /// Blocks user by Id.
        /// </summary>
        /// <param name="id"> Id of the user. </param>
        

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("api/user/{id}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            try
            {
                bool success = await userService.BlockByIdAsync(id);
                if (success)
                {
                    var okMessage = $"Succesfully blocked user id: {id}.";
					return Ok(okMessage);
                    //tracer.Info(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, okMessage);
                }
            }
            catch (Exception e)
            {
				return StatusCode(500);
               // tracer.Error(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, e);
            }
			return NoContent();
            //tracer.Warn(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, message);
        }

        /// <summary>
        /// Search for user with match in first or lastname with role criteria.
        /// </summary>
        /// <param name="key"> String to match. </param>
        /// <param name="role"> Role criteria. </param>
       

        [Authorize(Roles = "Admin, Mentor")]
        [HttpGet]
        [Route("api/user/search")]
        public async Task<ActionResult> SearchAsync(string key, string role)
        {
            if (key == null)
            {
                key = "";
            }

            RoleDto criteria = await  roleService.GetByNameAsync(role);
            var lines = key.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            int? searchParametr = null;
            if (role == Constants.Roles.Blocked)
            {
                searchParametr = Constants.Roles.BlockedIndex;
            }
            if (lines.Length > 2)
            {
                lines = lines.Take(2).ToArray();
            }
            List<UserDto> users =  criteria != null ? await userService.SearchAsync(lines, criteria.Id) :
                await userService.SearchAsync(lines, searchParametr);
            if ( users.Count != 0)
            {
				return Ok(users);
            }
			return NoContent();
        }

        /// <summary>
        /// Search for user with match in first or lastname with role criteria.
        /// </summary>
        /// <param name="key"> String to match. </param>
        /// <param name="role"> Role criteria. </param>
        /// <param name="pageSize"> Ammount of users that you want to see on one page</param>
        /// <param name="pageNumber"> Page number</param>
        

        [Authorize(Roles = "Admin, Mentor")]
        [HttpGet]
        [Route("api/user/search")]
        public async Task<ActionResult> SearchAsync(string key, string role, [FromQuery]int pageSize, [FromQuery]int pageNumber)
        {
            if (key == null)
            {
                key = "";
            }
            var criteria = await roleService.GetByNameAsync(role);
            var lines = key.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            int? searchParametr = null;
            if (role == Constants.Roles.Blocked)
            {
                searchParametr = Constants.Roles.BlockedIndex;
            }
            if (lines.Length > 2)
            {
                lines = lines.Take(2).ToArray();
            }
            var users = criteria != null ? await userService.SearchAsync(lines, pageSize, pageNumber, criteria.Id) :

            await userService.SearchAsync(lines, pageSize, pageNumber, searchParametr);

			return Ok(users);
        }

        /// <summary>
        /// Updates user password by id.
        /// </summary>
        /// <param name="password"> New password value. </param>
        /// <param name="id"> Users Id. </param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPut]
        [Route("api/user/resetpasswotd")]
        public async Task<ActionResult> ResetPasswordAsync([FromBody]string password, int id)
        {
            try
            {
                bool success = await userService.UpdatePasswordAsync(id, password);
                if (success)
                {
                    const string okMessage = "Succesfully updated password.";
                   //tracer.Info(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, okMessage);
                }
				return NoContent();
            }
            catch (Exception e)
            {
				return StatusCode(500);
                //tracer.Error(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, e);
            }
        }

        /// <summary>
        /// Updates current user password.
        /// </summary>
        /// <param name="value"> New password value. </param>
        /// <returns></returns>

        [HttpPut]
        [Route("api/user/newpassword")]
        public async Task<ActionResult> UpdatePasswordAsync([FromBody]string value)
        {
            int id = userIdentityService.GetUserId();
            return await ResetPasswordAsync(value, id);
        }

        /// <summary>
        /// Returns all roles of the users.
        /// </summary>

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("api/user/roles")]
        public async Task<ActionResult> GetRoles()
        {
            var roles = await roleService.GetAllRoles();
            if (roles.Count != 0)
            {
				return Ok(roles);
            }
			return NoContent();
        }

        /// <summary>
        /// Releases memory
        /// </summary>
        protected void Dispose(bool disposing)
        {
            userService.Dispose();
            roleService.Dispose();
            base.Dispose(disposing);
        }
    }
}
