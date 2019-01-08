using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LearnWithMentor.BLL.Interfaces;
using LearnWithMentor.DAL.Repositories.Interfaces;
using LearnWithMentor.Services;
using LearnWithMentorBLL.Interfaces;
using LearnWithMentorDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace LearnWithMentor.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class TaskDiscussionController : Controller
    {
        private readonly ITaskDiscussionService _taskDiscussionService;
        private readonly IUserService _userService;
        private readonly IUserIdentityService _userIdentityService;
        private readonly IHubContext<NotificationController, IHubClient> _chatHubContext;

        public TaskDiscussionController(ITaskDiscussionService taskDiscussionService, 
            IUserIdentityService userIdentityService, 
            IUserService userService,
        IHubContext<NotificationController, IHubClient> chatHub)
        {
            _taskDiscussionService = taskDiscussionService;
            _userService = userService;
            _userIdentityService = userIdentityService;
            _chatHubContext = chatHub;
        }
      
        [HttpGet]
        [Route("api/task/discussion/{taskId}")]
        public async Task<IActionResult> GetTaskDiscussionAsync(int taskId)
        {
            try
            {              
                List<TaskDiscussionWithNamesDTO> taskDiscussionWithNames = new List<TaskDiscussionWithNamesDTO>();
                var taskDiscussion = await _taskDiscussionService.GetTaskDiscussionAsync(taskId);
                foreach (var message in taskDiscussion)
                {
                    var user = await _userService.GetAsync(message.SenderId);
                    taskDiscussionWithNames.Add(new TaskDiscussionWithNamesDTO(user.FirstName+" "+user.LastName, message));
                }          
                return new JsonResult(taskDiscussionWithNames);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("api/task/discussion/{taskId}")]
        [HttpPost]
        public async Task<IActionResult> AddMessageToTaskDiscussion(int taskId, [FromBody]TaskDiscussionDTO taskDiscussion)
        {
            try
            {               
                var userId = _userIdentityService.GetUserId();
                var dt = DateTime.Now;
                dt = new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, 0);
                await _taskDiscussionService.AddTaskDiscussionAsync(userId, taskId, taskDiscussion.Text, dt);
                await _chatHubContext.Clients.All.TaskDiscussionMessage();
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}