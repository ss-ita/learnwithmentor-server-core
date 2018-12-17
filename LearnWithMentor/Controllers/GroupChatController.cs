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
    [AllowAnonymous]
    [EnableCors(Constants.Cors.policyName)]
    public class GroupChatController : Controller
    {
        private readonly IHubContext<NotificationController, IHubClient> _chatHubContext;
        private readonly IGroupService _groupService;
        private readonly IUserService _userService;
        private readonly IUserIdentityService _userIdentityService;
        private readonly IGroupChatService _groupChatService;

        public GroupChatController(IGroupService groupService, IUserService userService,
            IUserIdentityService userIdentityService, IHubContext<NotificationController, IHubClient> chatHub, IGroupChatService groupChatService)
        {
            this._userService = userService;
            this._groupService = groupService;
            this._userIdentityService = userIdentityService;
            this._chatHubContext = chatHub;
            this._groupChatService = groupChatService;
        }

        [Route("api/chat/{id}/{message}")]
        [HttpGet]
        public async Task<IActionResult> SendToAll(int id, string message)
        {
            try
            {
                var user = await _userService.GetAsync(id);
                await _chatHubContext.Clients.All.SendMessage(user.FirstName, message, DateTime.Now.ToString("h:mm:ss tt"));
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
                    await _chatHubContext.Clients.Group(groups.First().ToString()).SendMessage(user.FirstName, message, DateTime.Now.ToString("h:mm:ss tt"));
                    await _groupChatService.AddGroupChatMessageAsync(user.Id, groups.First(), message, DateTime.Now);
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
                string userConnectionId = NotificationController.ConnectedUsers[user.FirstName + " " + user.LastName];
                string groupId = groups.First().ToString();
                await _chatHubContext.Groups.AddToGroupAsync(userConnectionId, groupId);
                await _chatHubContext.Clients.Group(groups.First().ToString()).SendMessage(user.FirstName, "Connected to group", DateTime.Now.ToString("h:mm:ss tt"));
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
                    await _chatHubContext.Clients.Client(NotificationController.ConnectedUsers[currentUser.FirstName + " " + currentUser.LastName]).SendMessage(user.FirstName, groupChatMessage.TextMessage,
                        DateTime.Now.ToString("h:mm:ss tt"));
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