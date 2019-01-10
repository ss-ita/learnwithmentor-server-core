using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using LearnWithMentor.BLL.Interfaces;
using LearnWithMentor.Services;
using LearnWithMentorBLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using LearnWithMentorDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using LearnWithMentor.DAL.Entities;

namespace LearnWithMentor.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [EnableCors(Constants.Cors.policyName)]
    public class GroupChatController : Controller
    {
        private readonly IHubContext<NotificationController, IHubClient> _chatHubContext;
        private readonly IGroupService _groupService;
        private readonly IUserService _userService;
        private readonly IGroupChatService _groupChatService;
        private readonly INotificationService _notificationService;
        private readonly IUserIdentityService _userIdentityService;        

        public static List<string> clients = new List<string>();

        public GroupChatController(
            IGroupService groupService, 
            IUserService userService,
            IGroupChatService groupChatService,
            INotificationService notificationService,
            IUserIdentityService userIdentityService, 
            IHubContext<NotificationController, IHubClient> chatHub)
        {
            this._userService = userService;
            this._groupService = groupService;
            this._groupChatService = groupChatService;
            this._notificationService = notificationService;
            this._userIdentityService = userIdentityService;
            this._chatHubContext = chatHub;
        }

        [Route("api/chat/{id}/{message}")]
        [HttpGet]
        public async Task<IActionResult> SendToAll(int id, string message)
        {
            try
            {
                var user = await _userService.GetAsync(id);
                await _chatHubContext.Clients.All.SendMessage(id, user.FirstName, message,
                    DateTime.Now.ToString("h:mm:ss tt"));
                return Ok();

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("api/chat/{id}/{message}/group")]
        [HttpGet]
        public async Task<IActionResult> SendToGroup(int id, string message)
        {
            try
            {
                var user = await _userService.GetAsync(id);
                var groups = await _groupService.GetUserGroupsIdAsync(id);

                if (groups.Count() == 1)
                {
                    await _chatHubContext.Clients.Group(groups.First().ToString())
                        .SendMessage(id, user.FirstName, message, DateTime.Now.ToString("h:mm:ss tt"));
                    await _groupChatService.AddGroupChatMessageAsync(user.Id, groups.First(), message, DateTime.Now);

                    string notificationText = "You have new messages from your group";
                    NotificationType notificationType = NotificationType.NewMessage;
                    var users = await _groupService.GetUsersAsync(groups.FirstOrDefault());

                    foreach (var userReciever in users)
                    {
                        if (userReciever.Id != user.Id && !NotificationController.ConnectedUsers.ContainsKey(userReciever.Id.ToString()))
                        {
                            await _notificationService.AddNotificationAsync(notificationText, notificationType, DateTime.Now, userReciever.Id);
                        }
                    }
                }

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("api/chat/connect/{id}")]
        [HttpGet]
        public async Task<IActionResult> AddToGroup(int id)
        {
            try
            {
                var user = await _userService.GetAsync(id);
                var groups = await _groupService.GetUserGroupsIdAsync(id);
                string userConnectionId = NotificationController.ConnectedUsers[user.Id.ToString()];
                string groupId = groups.First().ToString();
                await _chatHubContext.Groups.AddToGroupAsync(userConnectionId, groupId);
                await _chatHubContext.Clients.Group(groups.First().ToString())
                    .SendMessage(id, user.FirstName, "Connected to group", DateTime.Now.ToString("h:mm:ss tt"));
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("api/chat/getmessages/{userId}")]
        [HttpGet]
        public async Task<IActionResult> GetMessages(int userId)
        {
            try
            {
                var groups = await _groupService.GetUserGroupsIdAsync(userId);
                var messages = await _groupChatService.GetGroupMessagesAsync(groups.First());
                var currentUser = await _userService.GetAsync(userId);

                foreach (var groupChatMessage in messages)
                {
                    var user = await _userService.GetAsync(groupChatMessage.SenderId);
                    await _chatHubContext.Clients
                        .Client(NotificationController.ConnectedUsers[user.Id.ToString()])
                        .SendMessage(userId, user.FirstName, groupChatMessage.TextMessage, groupChatMessage.Time.ToString());
                }

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("api/chat/getmessages/{userId}/{amount}")]
        [HttpGet]
        public async Task<IActionResult> GetMessages(int userId, int amount)
        {
            try
            {
                var groups = await _groupService.GetUserGroupsIdAsync(userId);
                var messages = await _groupChatService.GetGroupMessagesAsync(groups.First(), amount);
                var currentUser = await _userService.GetAsync(userId);

                foreach (var groupChatMessage in messages.Reverse())
                {
                    var user = await _userService.GetAsync(groupChatMessage.SenderId);
                    await _chatHubContext.Clients
                        .Client(NotificationController.ConnectedUsers[currentUser.Id.ToString()])
                        .SendMessage(userId, user.FirstName, groupChatMessage.TextMessage, groupChatMessage.Time.ToString());
                }

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}