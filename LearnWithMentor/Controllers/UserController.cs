using LearnWithMentorDTO;
using LearnWithMentorBLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;
using LearnWithMentor.Services;
using LearnWithMentor.Models;
using Microsoft.AspNetCore.Http;
using LearnWithMentor.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using System.Net.Http;
using LearnWithMentor.Constants;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using LearnWithMentor.Logger;
using System.Net;

namespace LearnWithMentor.Controllers
{
    /// <summary>
    /// Controller for system users.
    /// </summary>
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class UserController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly RoleManager<Role> roleManager;
        private static readonly HttpClient Client = new HttpClient();

        private readonly IUserService userService;
        private readonly IRoleService roleService;
        private readonly ITaskService taskService;
        private readonly IUserIdentityService userIdentityService;
		private readonly IHttpContextAccessor _accessor;
        private readonly ILogger logger;

		/// <summary>
		/// Creates an instance of UserController.
		/// </summary>
		public UserController(IUserService userService, IRoleService roleService, ITaskService taskService, IUserIdentityService userIdentityService, IHttpContextAccessor accessor, UserManager<User> userManager, RoleManager<Role> roleManager, ILoggerFactory loggerFactory)
        {
            this.userService = userService;
            this.roleService = roleService;
            this.taskService = taskService;
            this.userIdentityService = userIdentityService;
			this._accessor = accessor;
            this.userManager = userManager;
            this.roleManager = roleManager;

			loggerFactory.AddFile(Path.Combine(Directory.GetCurrentDirectory(), Constants.Logger.logFileName));
			logger = loggerFactory.CreateLogger("FileLogger");
		}

		/// <summary>
		/// Returns all users of the system.
		/// </summary>
		[Authorize(Roles = "Admin, Mentor")]
        [HttpGet]
        [Route("api/user")]
        public async Task<ActionResult> GetAsync()
        {
            List<UserDTO> users = await userService.GetAllUsersAsync();
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
        [Route("api/user/pagesize/{pageSize}/pagenumber/{pageNumber}")]
        public async Task<ActionResult> GetAsync(int pageSize, int pageNumber)
        {
            try
            {
                PagedListDTO<UserDTO> users = await userService.GetUsers(pageSize, pageNumber);
				return Ok(users);
            }
            catch (Exception e)
            {
				logger.LogInformation("Error :  {0}", e.Message);
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
            List<UserDTO> users = await userService.GetUsersByRoleAsync(roleId);
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
        [Route("api/user/inrole/{roleId}/pagesize/{pageSize}/pagenumber/{pageNumber}")]
        public async Task<ActionResult> GetUsersbyRoleAsync(int roleId, int pageSize, int pageNumber)
        {
            if (roleId != Constants.Roles.BlockedIndex)
            {
                var role = await roleService.GetAsync(roleId);
                if (role == null)
                {
					return NoContent();
                }
            }
            PagedListDTO<UserDTO> users = await userService.GetUsersByRoleAsync(roleId, pageSize, pageNumber);
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
            List<UserDTO> users = await userService.GetUsersByStateAsync(state);
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
        [Route("api/user/instate/{state}/pagesize/{pageSize}/pagenumber/{pageNumber}")]
        public async Task<ActionResult> GetUsersbyStateAsync(bool state, int pageSize, int pageNumber)
        {
            PagedListDTO<UserDTO> users = await userService.GetUsersByStateAsync(state, pageSize, pageNumber);
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
            var userex = _accessor.HttpContext.User;
            if (id == 0)
            {
                id = userIdentityService.GetUserId();
            }

            UserDTO user = await userService.GetAsync(id);
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
        public async Task<ActionResult> PostAsync([FromBody]UserRegistrationDTO value)
        {
            if (!ModelState.IsValid)
            {
				return BadRequest(ModelState);
            }
            try
            {
                var user_role = await roleManager.FindByNameAsync("Student");
                var userIdentity = new User()
                {
                    Email = value.Email,
                    FirstName = value.FirstName,
                    LastName = value.LastName,
                    UserName = value.Email,
                    Role = user_role,
                    Role_Id = user_role.Id,
                    EmailConfirmed = false
                };

                var result = await userManager.CreateAsync(userIdentity, value.Password);
                if (result.Succeeded)
                {
                    var add_role = await userManager.AddToRoleAsync(userIdentity, "Student");
                    if (add_role.Succeeded)
                    {
                        var okMessage = $"Succesfully created user: {value.Email}.";
                        return new JsonResult(okMessage);
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
                else
                {
                    const string message = "Incorrect request syntax.";
                    return BadRequest(message);
                }

            }
            catch (Exception e)
            {
                logger.LogInformation("Error :  {0}", e.Message);
                return StatusCode(500);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("api/user/facebook")]
        public async Task<ActionResult> FacebookPost([FromBody]FacebookDTO value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var appAccessTokenResponse = await Client.GetStringAsync($"https://graph.facebook.com/oauth/access_token?client_id={Facebook.AppId}&client_secret={Facebook.AppSecret}&grant_type=client_credentials");
                var appAccessToken = JsonConvert.DeserializeObject<FacebookAppAccessToken>(appAccessTokenResponse);

                var userAccessTokenValidationResponse = await Client.GetStringAsync($"https://graph.facebook.com/debug_token?input_token={value.AccessToken}&access_token={appAccessToken.AccessToken}");
                var userAccessTokenValidation = JsonConvert.DeserializeObject<FacebookUserAccessTokenValidation>(userAccessTokenValidationResponse);

                if (!userAccessTokenValidation.Data.IsValid)
                {
                    return BadRequest("Invalid facebook token!");
                }

                var userInfoResponse = await Client.GetStringAsync($"https://graph.facebook.com/v3.2/me?fields=id,email,first_name,last_name,name,picture&access_token={value.AccessToken}");
                var userInfo = JsonConvert.DeserializeObject<FacebookUserData>(userInfoResponse);

                var user = await userManager.FindByEmailAsync(userInfo.Email);

                if (user == null)
                {
                    var userRole = await roleManager.FindByNameAsync("Student");
                    string picture = Convert.ToBase64String(GetImgBytesAsync(userInfo.Picture.Data.Url).Result);
                    User newUser = new User
                    {
                        FirstName = userInfo.FirstName,
                        LastName = userInfo.LastName,
                        Email = userInfo.Email,
                        UserName = userInfo.Email,
                        Image = picture,
                        Image_Name = userInfo.Name + "_Picture",
                        Role = userRole,
                        Role_Id = userRole.Id,
                        EmailConfirmed = true
                    };
                    var result = await userManager.CreateAsync(newUser, Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Substring(0, 8));

                    if (!result.Succeeded)
                    {
                        return BadRequest();
                    }
                }

                var localUser = await userManager.FindByNameAsync(userInfo.Email);

                if (localUser == null)
                {
                    return BadRequest("Failed to create local user account.");
                }

                var userDto = new UserIdentityDTO()
                {
                    Email = localUser.Email,
                    LastName = localUser.LastName,
                    FirstName = localUser.FirstName,
                    Id = localUser.Id,
                    Role = localUser.Role.Name,
                    EmailConfirmed = localUser.EmailConfirmed,
                    Blocked = localUser.Blocked,
                    Password = localUser.Password
                };

                string jwt = JwtManager.GenerateToken(userDto);
                return new JsonResult(jwt);
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("api/user/google")]
        public async Task<ActionResult> GooglePost([FromBody]GoogleDTO value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var validateToken = await Client.GetStringAsync($"https://www.googleapis.com/oauth2/v1/tokeninfo?access_token={value.AccessToken}");
                var userInfoResponse = await Client.GetStringAsync($"https://www.googleapis.com/oauth2/v1/userinfo?access_token={value.AccessToken}");
                var userInfo = JsonConvert.DeserializeObject<GoogleUserData>(userInfoResponse);
                var user = await userManager.FindByEmailAsync(userInfo.Email);

                if (user == null)
                {
                    var userRole = await roleManager.FindByNameAsync("Student");
                    string picture = Convert.ToBase64String(GetImgBytesAsync(userInfo.Picture).Result);
                    User newUser = new User
                    {
                        FirstName = userInfo.FirstName,
                        LastName = userInfo.LastName,
                        Email = userInfo.Email,
                        UserName = userInfo.Email,
                        Image = picture,
                        Image_Name = userInfo.Name + "_Picture",
                        Role = userRole,
                        Role_Id = userRole.Id,
                        EmailConfirmed = true
                    };
                    var result = await userManager.CreateAsync(newUser, Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Substring(0, 8));

                    if (!result.Succeeded)
                    {
                        return BadRequest();
                    }
                }

                var localUser = await userManager.FindByNameAsync(userInfo.Email);

                if (localUser == null)
                {
                    return BadRequest("Failed to create local user account.");
                }

                var userDto = new UserIdentityDTO()
                {
                    Email = localUser.Email,
                    LastName = localUser.LastName,
                    FirstName = localUser.FirstName,
                    Id = localUser.Id,
                    Role = localUser.Role.Name,
                    EmailConfirmed = localUser.EmailConfirmed,
                    Blocked = localUser.Blocked,
                    Password = localUser.Password
                };

                string jwt = JwtManager.GenerateToken(userDto);
                return new JsonResult(jwt);
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }
        }

        private async static Task<byte[]> GetImgBytesAsync(string imgUrl)
        {
            WebClient webClient = new WebClient();
            var data = await webClient.DownloadDataTaskAsync(new Uri(imgUrl));
            return data;
        }

        private byte[] GetImgBytes(string imgUrl)
        {
            byte[] imageBytes;
            HttpWebRequest imageRequest = (HttpWebRequest)WebRequest.Create(imgUrl);
            WebResponse imageResponse = imageRequest.GetResponse();

            Stream responseStream = imageResponse.GetResponseStream();
            
            using (BinaryReader br = new BinaryReader(responseStream))
            {
                imageBytes = br.ReadBytes(1000000000);
                br.Close();
            }
            responseStream.Close();
            imageResponse.Close();

            return imageBytes;
        }

        /// <summary>
        /// Verifies reset password token.
        /// </summary>
        /// <param name="token"> Users token. </param>
        /// 


        [AllowAnonymous]
        [HttpGet]
        [Route("api/user/verify-token")]
        public async Task<ActionResult> VerifyTokenAsync(string token)
        {
            try
            {
                if (JwtManager.ValidateToken(token, out string userEmail, out string userrole))
                {
                    UserIdentityDTO user = await userService.GetByEmailAsync(userEmail);
                    if (user == null)
                    {
                        return BadRequest("User not found");
                    }
                    return Ok(user.Id);
                }
                return BadRequest("Token no longer valid");
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Confirms user email by token.
        /// </summary>
        /// <param name="token"> Users token. </param>
        [AllowAnonymous]
        [HttpGet]
        [Route("api/user/confirm-email")]
        public async Task<ActionResult> ConfirmEmailAsync(string token)
        {
            try
            {
                if (JwtManager.ValidateToken(token, out string userEmail, out string userrole))
                {
                    UserIdentityDTO user = await userService.GetByEmailAsync(userEmail);
                    if (user == null)
                    {
                        return BadRequest("User not found");
                    }
                    if (await userService.ConfirmEmailByIdAsync(user.Id))
                    {
                        User confirm_user = await userManager.FindByEmailAsync(userEmail);
                        var token_result = await userManager.GenerateEmailConfirmationTokenAsync(confirm_user);
                        var result = await userManager.ConfirmEmailAsync(confirm_user, token_result);
                        return Ok("Email confirmed");
                    }
                    return BadRequest("Confirmation error");
                }
                return BadRequest("Token no longer valid");
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Sends email with link for user's password reset.
        /// </summary>
        /// <param name="emailModel"> User's email. </param>
        /// <param name="resetPasswordLink"> Link on the reset page. </param>
        [AllowAnonymous]
        [HttpPost]
        [Route("api/user/password-reset")]
        public async Task<ActionResult> SendPasswordResetLinkAsync([FromBody] EmailDTO emailModel, string resetPasswordLink)
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
                    UserIdentityDTO user = await userService.GetByEmailAsync(emailModel.Email);
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
				logger.LogInformation("Error :  {0}", e.Message);
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
        public async Task<ActionResult> SendEmailConfirmLinkAsync([FromBody] EmailDTO emailModel, string emailConfirmLink)
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
                    UserIdentityDTO user = await userService.GetByEmailAsync(emailModel.Email);
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
				logger.LogInformation("Error :  {0}", e.Message);
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
            StatisticsDTO statistics = await taskService.GetUserStatisticsAsync(id);
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
				logger.LogInformation("Error :  {0}", e.Message);
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
                ImageDTO dto = await userService.GetImageAsync(id);
                if (dto == null)
                {
					return NoContent();
                }
				return Ok(dto);
            }
            catch (Exception e)
            {
				logger.LogInformation("Error :  {0}", e.Message);
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
        public async Task<ActionResult> PutAsync(int id, [FromBody]UserDTO value)
        {
            try
            {
                bool success = await userService.UpdateByIdAsync(id, value);
				if (success)
				{
					var okMessage = $"Succesfully updated user id: {id}.";
                    logger.LogInformation("Ok :  {0}", okMessage);
                    return Ok(okMessage);
				}
            }
            catch (Exception e)
            {
                logger.LogInformation("Error :  {0}", e.Message);
                return StatusCode(500);
            }
            const string message = "Incorrect request syntax or user does not exist.";
            logger.LogInformation("Error :  {0}", message);
            return BadRequest(message);
		}		

        [HttpPut]
        [Route("api/user/update-multiple")]
        public async Task<ActionResult> UpdateUsersAsync([FromBody]UserDTO[] value)
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
                    logger.LogInformation("Error :  {0}", okMessage);
                    return Ok(okMessage);
                }
            }
            catch (Exception e)
            {
				logger.LogInformation("Error :  {0}", e.Message);
				return StatusCode(500);
            }
            const string message = "Incorrect request syntax or users do not exist.";
			logger.LogInformation("Error :  {0}", message);
			return BadRequest(message);
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
					logger.LogInformation("Error :  {0}", okMessage);
					return Ok(okMessage);
                }
            }
            catch (Exception e)
            {
				logger.LogInformation("Error :  {0}", e.Message);
				return StatusCode(500);
            }
			return NoContent();
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

            RoleDTO criteria = await  roleService.GetByNameAsync(role);
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
            List<UserDTO> users =  criteria != null ? await userService.SearchAsync(lines, criteria.Id) :
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
        public async Task<ActionResult> ResetPasswordAsync([FromBody]ResetPasswordDTO value, int id)
        {
            try
            {
                User user = await userManager.FindByIdAsync(id.ToString());
                var result = await userManager.ChangePasswordAsync(user, value.OldPassword, value.NewPassword);
                if (result.Succeeded)
                {
                    const string okMessage = "Succesfully updated password.";
                    logger.LogInformation("Error :  {0}", okMessage);
                    return Ok(okMessage);
				}
				return NoContent();
            }
            catch (Exception e)
            {
				logger.LogInformation("Error :  {0}", e.Message);
				return StatusCode(500);
            }
        }

        /// <summary>
        /// Updates current user password.
        /// </summary>
        /// <param name="value"> New password value. </param>
        /// <returns></returns>

        [HttpPut]
        [Route("api/user/newpassword")]
        public async Task<ActionResult> UpdatePasswordAsync([FromBody]ResetPasswordDTO value)
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
        protected override void Dispose(bool disposing)
        {
            roleManager.Dispose();
            userManager.Dispose();
            userService.Dispose();
            roleService.Dispose();
            base.Dispose(disposing);
        }
    }
}
