using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LearnWithMentor.BLL.Interfaces;
using LearnWithMentor.DAL.Repositories.Interfaces;
using LearnWithMentorBLL.Interfaces;
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
                var taskDiscussion = await _taskDiscussionService.GetTaskDiscussionAsync(taskId);
                return new JsonResult(taskDiscussion);
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
                var user = await _userService.GetAsync(userId);
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