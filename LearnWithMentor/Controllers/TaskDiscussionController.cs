using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LearnWithMentor.BLL.Interfaces;
using LearnWithMentor.DAL.Repositories.Interfaces;
using LearnWithMentorBLL.Interfaces;
using LearnWithMentorDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearnWithMentor.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class TaskDiscussionController : Controller
    {
        private readonly ITaskDiscussionService _taskDiscussionService;
        private readonly IUserService _userService;
        private readonly IUserIdentityService _userIdentityService;

        public TaskDiscussionController(ITaskDiscussionService taskDiscussionService, IUserIdentityService userIdentityService, IUserService userService)
        {
            _taskDiscussionService = taskDiscussionService;
            _userService = userService;
            _userIdentityService = userIdentityService;
        }

        [HttpGet]
        [Route("api/task/discussion/{taskId}")]
        public async Task<IActionResult> GetTaskDiscussionAsync(int taskId)
        {
            try
            {
                Dictionary<string, TaskDiscussionDTO> taskDiscussionWithNames = new Dictionary<string, TaskDiscussionDTO>();          
                var taskDiscussion = await _taskDiscussionService.GetTaskDiscussionAsync(taskId);
                foreach (var message in taskDiscussion)
                {
                    var user = await _userService.GetAsync(message.SenderId);
                    taskDiscussionWithNames.Add(user.FirstName + " " + user.LastName, message);
                }          
                return new JsonResult(taskDiscussionWithNames);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("api/task/discussion/{taskId}/{text}")]
        [HttpPost]
        public async Task<IActionResult> AddMessageToTaskDiscussion(int taskId, string text)
        {
            try
            {
                var userId = _userIdentityService.GetUserId();
                await _taskDiscussionService.AddTaskDiscussionAsync(userId, taskId, text, DateTime.Now);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}