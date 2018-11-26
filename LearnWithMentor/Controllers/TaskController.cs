using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Http;
using System.Net;
using System.Net.Http;
using LearnWithMentorDTO;
using LearnWithMentorBLL.Interfaces;
using System.Text.RegularExpressions;
using LearnWithMentorDTO.Infrastructure;
using System.Threading.Tasks;
using LearnWithMentor.Logger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;

namespace LearnWithMentor.Controllers
{
    /// <summary>
    /// Controller for working with tasks.
    /// </summary>
    [Authorize]
    public class TaskController : Controller
    {
        /// <summary>
        /// Services for work with different DB parts.
        /// </summary>
        private readonly ITaskService taskService;
        private readonly IMessageService messageService;
        private readonly IUserIdentityService userIdentityService;
        private readonly ILogger logger;

        /// <summary>
        /// Services initiation.
        /// </summary>
        public TaskController(ITaskService taskService, IMessageService messageService, IUserIdentityService userIdentityService, ILoggerFactory loggerFactory)
        {
            this.taskService = taskService;
            this.messageService = messageService;
            this.userIdentityService = userIdentityService;
            loggerFactory.AddFile(Path.Combine(Directory.GetCurrentDirectory(), "logger.txt"));
            logger = loggerFactory.CreateLogger("FileLogger");
        }

        /// <summary>
        /// Returns a list of all tasks in database.
        /// </summary>
        [HttpGet]
        [Route("api/task")]
        public async Task<ActionResult> GetAllTasksAsync()
        {
            try
            {
                var allTasks = await taskService.GetAllTasksAsync();
                if (allTasks != null)
                {
                    return Ok(allTasks);
                }

                return NoContent();
            }
            catch (Exception e)
            {
                logger.LogInformation("Eror Exeption: {0}", e.Message);
                return BadRequest();
            }
        }

        /// <summary>
        /// Returns a list of all tasks in database.
        /// </summary>
        [HttpGet]
        [Route("api/task/pageSize/{pageSize}/pageNumber/{pageNumber}")]
        public ActionResult GetTasks(int pageSize, int pageNumber)
        {
            try
            {
                var tasks = taskService.GetTasks(pageSize, pageNumber);
                if (tasks != null)
                {
                    return Ok(tasks);
                }

                return NoContent();
            }
            catch (Exception e)
            {
                //TODO: Log error here
                return BadRequest();
            }
        }

        /// <summary>
        /// Returns a list of all tasks not used in current plan.
        /// </summary>
        /// <param name="planId">Id of the plan.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/plan/{planId}/tasks/notinplan")]
        public async Task<ActionResult> GetTasksNotInCurrentPlanAsync(int planId)
        {
            IEnumerable<TaskDto> task = await taskService.GetTasksNotInPlanAsync(planId);
            if (task != null)
            {
                return Ok(task);
            }
            var errorMessage = "There isn't tasks outside of the plan id = {planId}";
            return BadRequest(errorMessage);
        }

        /// <summary>
        /// Returns task by Id.
        /// </summary>
        [HttpGet]
        [Route("api/task/{taskId}")]
        public async Task<ActionResult> GetTaskByIdAsync(int taskId)
        {
            try
            {
                TaskDto task = await taskService.GetTaskByIdAsync(taskId);
                if (task == null)
                {
                    return NoContent();
                }
                return Ok(task);
            }
            catch (Exception e)
            {
                //TODO: Log error here
                return BadRequest();
            }
        }

        /// <summary>
        /// Returns tasks with priority and section for by PlanTask Id.
        /// </summary>
        /// <param name="planTaskId">Id of the planTask.</param>
        [HttpGet]
        [Route("api/task/plantask/{planTaskId}")]
        public async Task<ActionResult> GetTaskForPlanAsync(int planTaskId)
        {
            try
            {
                TaskDto task = await taskService.GetTaskForPlanAsync(planTaskId);
                if (task != null)
                {
                    return Ok(task);
                }
                return NoContent();
            }
            catch (Exception e)
            {
                //TODO: Log error here
                return BadRequest();
            }
        }

        /// <summary>
        /// Returns UserTask for task in plan for user.
        /// </summary>
        /// <param name="planTaskId">Id of the planTask.</param>
        /// <param name="userId">Id of the user.</param>
        [HttpGet]
        [Route("api/task/usertask")]
        public async Task<ActionResult> GetUserTaskAsync(int planTaskId, int userId)
        {
            try
            {
                var currentId = userIdentityService.GetUserId();
                var currentRole = userIdentityService.GetUserRole();
                if (!(userId == currentId || currentRole == Constants.Roles.Mentor))
                {
                    return BadRequest();
                }
                UserTaskDto userTask = await taskService.GetUserTaskByUserPlanTaskIdAsync(userId, planTaskId);
                if (userTask != null)
                {
                    return Ok(userTask);
                }
                return NoContent();
            }
            catch (Exception e)
            {
                //TODO: Log error here
                return BadRequest();
            }
        }

        /// <summary>
        /// Returns all UserTasks for array of user`s ids for specific plan tasks .
        /// </summary>
        /// <param name="planTaskId">array of the planTask`s ids.</param>
        /// <param name="userId">array of the user`s ids.</param>
        [HttpGet]
        [Route("api/task/allusertasks")]
        public async Task<ActionResult> GetUsersTasksAsync([FromQuery]int[] userId, [FromQuery]int[] planTaskId)
        {
            try
            {
                var allUserTasks = new List<ListUserTasksDto>();
                foreach (var userid in userId)
                {
                    var userTasks = await taskService.GetTaskStatesForUserAsync(planTaskId, userid);
                    if (userTasks == null)
                    {
                        return NoContent();
                    }
                    allUserTasks.Add(new ListUserTasksDto() { UserTasks = userTasks });
                }
                return Ok(allUserTasks);
            }
            catch (Exception e)
            {
                //TODO: Log error here
                return BadRequest();
            }
        }

        /// <summary>
        /// Returns all UserTasks for array of user`s ids for specific plan tasks .
        /// </summary>
        /// <param name="planTaskId">array of the planTask`s ids.</param>
        /// <param name="userId">array of the user`s ids.</param>
        [HttpGet]
        [Route("api/task/usertasks")]
        public async Task<ActionResult> GetUserTasksAsync(int userId, [FromQuery]int[] planTaskId)
        {
            try
            {
                List<UserTaskDto> userTasks = await taskService.GetTaskStatesForUserAsync(planTaskId, userId);
                if (userTasks == null)
                {
                    return NoContent();
                }
                return Ok(userTasks);
            }
            catch (Exception e)
            {
                //TODO: Log error here
                return BadRequest(e);
            }
        }

        /// <summary>Returns messages for UserTask by its id.</summary>
        /// <param name="userTaskId">Id of the usertask.</param>
        [HttpGet]
        [Route("api/task/userTask/{userTaskId}/messages")]
        public async Task<ActionResult> GetUserTaskMessagesAsync(int userTaskId)
        {
            try
            {
                var currentId = userIdentityService.GetUserId();
                var currentRole = userIdentityService.GetUserRole();
                if (!(await taskService.CheckUserTaskOwnerAsync(userTaskId, currentId) || currentRole == Constants.Roles.Mentor || currentRole == Constants.Roles.Admin))
                {
                    return BadRequest(HttpStatusCode.Unauthorized);
                }
                IEnumerable<MessageDto> messageList = await messageService.GetMessagesAsync(userTaskId);
                if (messageList != null)
                {
                    return Ok(messageList);
                }

                return NoContent();
            }
            catch (Exception e)
            {
                //TODO: Log error here
                return BadRequest(e);

            }
        }

        /// <summary> Creates message for UserTask. </summary>
        /// <param name="userTaskId">Id of the usertask.</param>
        /// <param name="newMessage">New message to be created.</param>
        /// <returns></returns>



        [HttpPost]
        [Route("api/task/userTask/{userTaskId}/messages")]
        public ActionResult PostUserTaskMessage(int userTaskId, [FromBody]MessageDto newMessage)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                newMessage.UserTaskId = userTaskId;
                var currentId = userIdentityService.GetUserId();
                newMessage.SenderId = currentId;
                var success = messageService.SendMessage(newMessage);
                if (success)
                {
                    var message = $"Succesfully created message with id = {newMessage.Id} by user with id = {newMessage.SenderId}";
                    //TODO: Log error here
                    return Ok(message);
                }
                //TODO: Log error here
                var erorMessage = "Creation error.";
                return BadRequest(erorMessage);
            }
            catch (Exception e)
            {
                //TODO: Log error here
                return BadRequest(e);
            }
        }

        /// <summary>
        /// Creates new UserTask.
        /// </summary>
        /// <param name="newUserTask">New userTask object.</param>
        [HttpPost]
        [Route("api/task/usertask")]
        public async Task<ActionResult> PostNewUserTaskAsync([FromBody]UserTaskDto newUserTask)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                bool success = await taskService.CreateUserTaskAsync(newUserTask);
                if (success)
                {
                    var message = $"Succesfully created task with id = {newUserTask.Id} for user with id = {newUserTask.UserId}";
                    //TODO: Log error here
                    return Ok(message);
                }
                //TODO: Log error here
                return NoContent();
            }
            catch (Exception e)
            {
                //TODO: Log error here
                return BadRequest(e);
            }
        }

        /// <summary>Changes UserTask status by usertask id.</summary>
        /// <param name="userTaskId">Id of the userTask status to be changed.</param>
        /// /// <param name="newStatus">New userTask.</param>
        [HttpPut]
        [Route("api/task/usertask/status")]
        public async Task<ActionResult> PutNewUserTaskStatusAsync(int userTaskId, string newStatus)
        {
            try
            {
                if (!Regex.IsMatch(newStatus, ValidationRules.USERTASK_STATE))
                {
                    return BadRequest();
                }
                var success = await taskService.UpdateUserTaskStatusAsync(userTaskId, newStatus);
                if (success)
                {
                    var message = $"Succesfully updated user task with id = {userTaskId} on status {newStatus}";
                    //TODO: Log error here
                    return Ok(message);
                }
                //TODO: Log error here
                var errorMessage = "Incorrect request syntax or usertask does not exist.";
                return BadRequest(errorMessage);
            }
            catch (Exception e)
            {
                //TODO: Log error here
                return BadRequest(e);
            }
        }

        /// <summary>
        /// <param name="userTaskId">Id of the userTask status to be changed.</param>
        /// <param name="NewMessage">New userTask.</param>
        /// </summary>
        [HttpPut]
        [Route("api/task/userTask/{userTaskId}/messages/isRead")]
        public async Task<ActionResult> PutNewIsReadValueAsync(int userTaskId, MessageDto NewMessage)
        {
            try
            {
                bool success = await messageService.UpdateIsReadStateAsync(userTaskId, NewMessage);
                if (success)
                {
                    var message = $"Succesfully updated usertask message isRead state with id = {userTaskId}";
                    //TODO: Log error here
                    return Ok(message);
                }
                //TODO: Log error here
                return NoContent();
            }
            catch (Exception e)
            {
                //TODO: Log error here
                return BadRequest(e);
            }
        }

        /// <summary> Changes UserTask result by usertask id. </summary>
        /// <param name="userTaskId">Id of the userTask status to be changed</param>
        /// <param name="newMessage">>New userTask result</param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/task/usertask/result")]
        public async Task<ActionResult> PutNewUserTaskResultAsync(int userTaskId, HttpRequestMessage newMessage)
        {
            try
            {
                var value = newMessage.Content.ReadAsStringAsync().Result;
                if (value.Length >= ValidationRules.MAX_USERTASK_RESULT_LENGTH)
                {
                    return BadRequest();
                }
                bool success = await taskService.UpdateUserTaskResultAsync(userTaskId, value);
                if (success)
                {
                    var message = $"Succesfully updated user task with id = {userTaskId} on result {value}";
                    //TODO: Log error here
                    return Ok(message);
                }
                //TODO: Log error here
                var errorMessage = "Incorrect request syntax or usertask does not exist.";
                return BadRequest(errorMessage);
            }
            catch (Exception e)
            {
                //TODO: Log error here
                return BadRequest(e);
            }
        }

        /// <summary>Returns tasks which name contains string key.</summary>
        /// <param name="key">Key for search.</param>
        [HttpGet]
        [Route("api/task/search")]
        public async Task<ActionResult> SearchAsync(string key)
        {
            try
            {
                if (string.IsNullOrEmpty(key))
                {
                    return await GetAllTasksAsync();
                }
                var lines = key.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                List<TaskDto> taskList = await taskService.SearchAsync(lines);
                if (taskList == null || taskList.Count == 0)
                {
                    return NoContent();
                }
                return Ok(taskList);
            }
            catch (Exception e)
            {
                //TODO: Log error here
                return BadRequest(e);
            }
        }

        /// <summary>
        /// Returns tasks in plan which names contain string key.
        /// </summary>
        /// <param name="key">Key for search.</param>
        /// <param name="planId">Id of the plan.</param>
        [HttpGet]
        [Route("api/task/searchinplan")]
        public async Task<ActionResult> SearchInPlanAsync(string key, int planId)
        {
            try
            {
                if (string.IsNullOrEmpty(key))
                {
                    return BadRequest();
                }
                var lines = key.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                IEnumerable<TaskDto> taskList = await taskService.SearchAsync(lines, planId);
                if (taskList == null)
                {
                    return NoContent();
                }
                return Ok(taskList);
            }
            catch (Exception e)
            {
                //TODO: Log error here
                return BadRequest(e);
            }
        }

        /// <summary>
        /// Creates new task
        /// </summary>
        /// <param name="newTask">Task object for creation.</param>
        [HttpPost]
        [Route("api/task")]
        public ActionResult Post([FromBody]TaskDto newTask)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(); 
                }
                bool success = taskService.CreateTask(newTask);
                if (success)
                {
                    var message = $"Succesfully created task with id = {newTask.Id} by user with id = {newTask.CreatorId}";
                    //TODO: Log error here
                    return Ok(message);
                }
                //TODO: Log error here
                return BadRequest();
            }
            catch (Exception e)
            {
                //TODO: Log error here
                return BadRequest(e);
            }

        }

        /// <summary>
        /// Creates new task and returns id of the created task.
        /// </summary>
        /// <param name="value"> New plan to be created. </param>
        [HttpPost]
        [Route("api/task/return")]
        public async Task<ActionResult> PostAndReturnIdAsync([FromBody]TaskDto value)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                int? result = await taskService.AddAndGetIdAsync(value);
                if (result != null)
                {
                    var log = $"Succesfully created task {value.Name} with id = {result} by user with id = {value.CreatorId}";
                    //TODO: Log error here
                    return Ok(result);
                }
            }
            catch (Exception e)
            {
                //TODO: Log error here
                return BadRequest(e);
            }
            //TODO: Log error here
            const string message = "Incorrect request syntax.";
            return BadRequest(message);
        }

        /// <summary>
        /// Updates task by Id
        /// </summary>
        /// <param name="taskId">Task Id for update.</param>
        /// <param name="task">Modified task object for update.</param>
        //[Authorize(Roles = "Mentor")]
        [HttpPut]
        [Route("api/task/{taskId}")]
        public async Task<ActionResult> PutAsync(int taskId, [FromBody]TaskDto task)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(); 
                }
                bool success = await taskService.UpdateTaskByIdAsync(taskId, task);
                if (success)
                {
                    var message = $"Succesfully updated task with id = {taskId}";
                    //TODO: Log error here
                    return Ok(message);
                }
                //TODO: Log error here
                return NoContent();
            }
            catch (Exception e)
            {
                //TODO: Log error here
                return BadRequest(e);
            }
        }

        /// <summary>
        /// Updates proposeEndDate by userTask Id
        /// </summary>
        /// <param name="userTaskId">UserTask Id for update.</param>
        /// <param name="proposeEndDate">New proposeEndDate</param>
        [Authorize]
        [HttpPut]
        [Route("api/task/usertask/proposedEndDate")]
        public async Task<ActionResult> PutAsync(int userTaskId, DateTime proposeEndDate)
        {
            try
            {
                bool success = await taskService.UpdateProposeEndDateAsync(userTaskId, proposeEndDate);
                if (success)
                {
                    var message = $"Succesfully updated usertask with id = {userTaskId}";
                    //TODO: Log error here
                    return Ok(message);
                }
                //TODO: Log error here
                return NoContent();
            }
            catch (Exception e)
            {
                //TODO: Log error here
                return BadRequest(e);
            }
        }

        /// <summary>
        /// Delete proposeEndDate for userTask
        /// </summary>
        /// <param name="userTaskId">UserTask Id for update.</param>
        [HttpDelete]
        [Route("api/task/usertask/proposedEndDate")]
        public async Task<ActionResult> DeleteProposeEndDateAsync(int userTaskId)
        {
            try
            {
                bool success = await taskService.DeleteProposeEndDateAsync(userTaskId);
                if (success)
                {
                    var message = $"Succesfully deleted proposeEndDate for usertask with id = {userTaskId}";
                    //TODO: Log error here
                    return Ok(message);
                }
                //TODO: Log error here
                return NoContent();
            }
            catch (Exception e)
            {
                //TODO: Log error here
                return BadRequest(e);
            }
        }

        /// <summary>
        /// Set new endDate for userTask
        /// </summary>
        /// <param name="userTaskId">UserTask Id for update.</param>
        [HttpPut]
        [Route("api/task/usertask/endDate")]
        public async Task<ActionResult> SetNewEndDateAsync(int userTaskId)
        {
            try
            {
                bool success = await taskService.SetNewEndDateAsync(userTaskId);
                if (success)
                {
                    var message = $"Succesfully changing endDate for usertask with id = {userTaskId}";
                    //TODO: Log error here
                    return Ok(message);
                }
                //TODO: Log error here
                return NoContent();
            }
            catch (Exception e)
            {
                //TODO: Log error here
                return BadRequest(e);
            }
        }

        /// <summary>
        /// Deletes task by Id
        /// </summary>
        /// <param name="taskId">Task Id for delete.</param>
        [HttpDelete]
        [Route("api/task/{id}")]
        public async Task<ActionResult> DeleteAsync(int taskId)
        {
            try
            {
                bool success = await taskService.RemoveTaskByIdAsync(taskId);
                if (success)
                {
                    var message = $"Succesfully deleted task with id = {taskId}";
                    //TODO: Log error here
                    return Ok(message);
                }
                //TODO: Log error here
                return BadRequest();
            }
            catch (Exception e)
            {
                //TODO: Log error here
                return BadRequest(e);
            }
        }

        /// <summary>
        /// Releases memory
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            taskService.Dispose();
            messageService.Dispose();
            base.Dispose(disposing);
        }
    }
}
