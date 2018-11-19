﻿using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Net;
using System.Net.Http;
using LearnWithMentorDTO;
using LearnWithMentorBLL.Interfaces;
using System.Text.RegularExpressions;
using LearnWithMentorDTO.Infrastructure;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearnWithMentor.Controllers
{
    /// <summary>
    /// Controller for working with tasks.
    /// </summary>
    [Authorize]
    public class TaskController : ApiController
    {
        /// <summary>
        /// Services for work with different DB parts.
        /// </summary>
        private readonly ITaskService taskService;
        private readonly IMessageService messageService;
        private readonly IUserIdentityService userIdentityService;

        /// <summary>
        /// Services initiation.
        /// </summary>
        public TaskController(ITaskService taskService, IMessageService messageService, IUserIdentityService userIdentityService)
        {
            this.taskService = taskService;
            this.messageService = messageService;
            this.userIdentityService = userIdentityService;
        }

        /// <summary>
        /// Returns a list of all tasks in database.
        /// </summary>
        [HttpGet]
        [Route("api/task")]
        public async Task<HttpResponseMessage> GetAllTasksAsync()
        {
            try
            {
                var allTasks = await taskService.GetAllTasksAsync();
                if (allTasks != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, allTasks);
                }
                return Request.CreateErrorResponse(HttpStatusCode.NoContent, "There are no tasks in database.");
            }
            catch (Exception e)
            {

                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }

        /// <summary>
        /// Returns a list of all tasks in database.
        /// </summary>
        [HttpGet]
        [Route("api/task/pageSize/{pageSize}/pageNumber/{pageNumber}")]
        public HttpResponseMessage GetTasks(int pageSize, int pageNumber)
        {
            try
            {
                var tasks = taskService.GetTasks(pageSize, pageNumber);
                if (tasks != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, tasks);
                }
                return Request.CreateErrorResponse(HttpStatusCode.NoContent, "There are no tasks in database.");
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }

        /// <summary>
        /// Returns a list of all tasks not used in current plan.
        /// </summary>
        /// <param name="planId">Id of the plan.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/plan/{planId}/tasks/notinplan")]
        public async Task<HttpResponseMessage> GetTasksNotInCurrentPlanAsync(int planId)
        {
            IEnumerable<TaskDto> task = await taskService.GetTasksNotInPlanAsync(planId);
            if (task != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, task);
            }
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, $"There isn't tasks outside of the plan id = {planId}");
        }

        /// <summary>
        /// Returns task by Id.
        /// </summary>
        [HttpGet]
        [Route("api/task/{taskId}")]
        public async Task<HttpResponseMessage> GetTaskByIdAsync(int taskId)
        {
            try
            {
                TaskDto task = await taskService.GetTaskByIdAsync(taskId);
                if (task == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NoContent, "This task does not exist in database.");
                }
                return Request.CreateResponse(HttpStatusCode.OK, task);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }

        /// <summary>
        /// Returns tasks with priority and section for by PlanTask Id.
        /// </summary>
        /// <param name="planTaskId">Id of the planTask.</param>
        [HttpGet]
        [Route("api/task/plantask/{planTaskId}")]
        public async Task<HttpResponseMessage> GetTaskForPlanAsync(int planTaskId)
        {
            try
            {
                TaskDto task = await taskService.GetTaskForPlanAsync(planTaskId);
                if (task != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, task);
                }
                return Request.CreateErrorResponse(HttpStatusCode.NoContent, "This task does not exist in database.");
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }

        /// <summary>
        /// Returns UserTask for task in plan for user.
        /// </summary>
        /// <param name="planTaskId">Id of the planTask.</param>
        /// <param name="userId">Id of the user.</param>
        [HttpGet]
        [Route("api/task/usertask")]
        public async Task<HttpResponseMessage> GetUserTaskAsync(int planTaskId, int userId)
        {
            try
            {
                var currentId = userIdentityService.GetUserId();
                var currentRole = userIdentityService.GetUserRole();
                if (!(userId == currentId || currentRole == Constants.Roles.Mentor))
                {
                    return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Authorization denied.");
                }
                UserTaskDto userTask = await taskService.GetUserTaskByUserPlanTaskIdAsync(userId, planTaskId);
                if (userTask != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, userTask);
                }
                return Request.CreateErrorResponse(HttpStatusCode.NoContent, "Task for this user does not exist in database.");
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }

        /// <summary>
        /// Returns all UserTasks for array of user`s ids for specific plan tasks .
        /// </summary>
        /// <param name="planTaskId">array of the planTask`s ids.</param>
        /// <param name="userId">array of the user`s ids.</param>
        [HttpGet]
        [Route("api/task/allusertasks")]
        public async Task<HttpResponseMessage> GetUsersTasksAsync([FromUri]int[] userId, [FromUri]int[] planTaskId)
        {
            try
            {
                var allUserTasks = new List<ListUserTasksDto>();
                foreach (var userid in userId)
                {
                    var userTasks = await taskService.GetTaskStatesForUserAsync(planTaskId, userid);
                    if (userTasks == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NoContent,
                            $"Task for this user with id: {userid}  does not exist in database.");
                    }
                    allUserTasks.Add(new ListUserTasksDto() { UserTasks = userTasks });
                }
                return Request.CreateResponse(HttpStatusCode.OK, allUserTasks);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }

        /// <summary>
        /// Returns all UserTasks for array of user`s ids for specific plan tasks .
        /// </summary>
        /// <param name="planTaskId">array of the planTask`s ids.</param>
        /// <param name="userId">array of the user`s ids.</param>
        [HttpGet]
        [Route("api/task/usertasks")]
        public async Task<HttpResponseMessage> GetUserTasksAsync(int userId, [FromUri]int[] planTaskId)
        {
            try
            {
                List<UserTaskDto> userTasks = await taskService.GetTaskStatesForUserAsync(planTaskId, userId);
                if (userTasks == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NoContent, $"Task for this user with id: {userId}  does not exist in database.");
                }
                return Request.CreateResponse(HttpStatusCode.OK, userTasks);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }

        /// <summary>Returns messages for UserTask by its id.</summary>
        /// <param name="userTaskId">Id of the usertask.</param>
        [HttpGet]
        [Route("api/task/userTask/{userTaskId}/messages")]
        public async Task<HttpResponseMessage> GetUserTaskMessagesAsync(int userTaskId)
        {
            try
            {
                var currentId = userIdentityService.GetUserId();
                var currentRole = userIdentityService.GetUserRole();
                if (!(await taskService.CheckUserTaskOwnerAsync(userTaskId, currentId) || currentRole == Constants.Roles.Mentor || currentRole == Constants.Roles.Admin))
                {
                    return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Authorization denied.");
                }
                IEnumerable<MessageDto> messageList = await messageService.GetMessagesAsync(userTaskId);
                if (messageList != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, messageList);
                }
                return Request.CreateErrorResponse(HttpStatusCode.NoContent, "Messages for this user does not exist in database.");
            }
            catch (Exception e)
            {

                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);

            }
        }

        /// <summary> Creates message for UserTask. </summary>
        /// <param name="userTaskId">Id of the usertask.</param>
        /// <param name="newMessage">New message to be created.</param>
        /// <returns></returns>



        [HttpPost]
        [Route("api/task/userTask/{userTaskId}/messages")]
        public HttpResponseMessage PostUserTaskMessage(int userTaskId, [FromBody]MessageDto newMessage)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                }
                newMessage.UserTaskId = userTaskId;
                var currentId = userIdentityService.GetUserId();
                newMessage.SenderId = currentId;
                var success = messageService.SendMessage(newMessage);
                if (success)
                {
                    var message = $"Succesfully created message with id = {newMessage.Id} by user with id = {newMessage.SenderId}";
                    return Request.CreateResponse(HttpStatusCode.OK, "Succesfully created message");
                }
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Creation error.");
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }

        /// <summary>
        /// Creates new UserTask.
        /// </summary>
        /// <param name="newUserTask">New userTask object.</param>
        [HttpPost]
        [Route("api/task/usertask")]
        public async Task<HttpResponseMessage> PostNewUserTaskAsync([FromBody]UserTaskDto newUserTask)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                }
                bool success = await taskService.CreateUserTaskAsync(newUserTask);
                if (success)
                {
                    var message = $"Succesfully created task with id = {newUserTask.Id} for user with id = {newUserTask.UserId}";
                    return Request.CreateResponse(HttpStatusCode.OK, "Succesfully created task for user.");
                }
                return Request.CreateErrorResponse(HttpStatusCode.NoContent, "There is no user or task in database");
            }
            catch (Exception e)
            {

                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }

        /// <summary>Changes UserTask status by usertask id.</summary>
        /// <param name="userTaskId">Id of the userTask status to be changed.</param>
        /// /// <param name="newStatus">New userTask.</param>
        [HttpPut]
        [Route("api/task/usertask/status")]
        public async Task<HttpResponseMessage> PutNewUserTaskStatusAsync(int userTaskId, string newStatus)
        {
            try
            {
                if (!Regex.IsMatch(newStatus, ValidationRules.USERTASK_STATE))
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "New Status not valid");
                }
                var success = await taskService.UpdateUserTaskStatusAsync(userTaskId, newStatus);
                if (success)
                {
                    var message = $"Succesfully updated user task with id = {userTaskId} on status {newStatus}";

                    return Request.CreateResponse(HttpStatusCode.OK, "Succesfully updated task for user.");
                }
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Incorrect request syntax or usertask does not exist.");
            }
            catch (Exception e)
            {

                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }

        /// <summary>
        /// <param name="userTaskId">Id of the userTask status to be changed.</param>
        /// <param name="NewMessage">New userTask.</param>
        /// </summary>
        [HttpPut]
        [Route("api/task/userTask/{userTaskId}/messages/isRead")]
        public async Task<HttpResponseMessage> PutNewIsReadValueAsync(int userTaskId, MessageDto NewMessage)
        {
            try
            {
                bool success = await messageService.UpdateIsReadStateAsync(userTaskId, NewMessage);
                if (success)
                {
                    var message = $"Succesfully updated usertask message isRead state with id = {userTaskId}";

                    return Request.CreateResponse(HttpStatusCode.OK, $"Succesfully updated usertask id: {userTaskId}.");
                }
                return Request.CreateErrorResponse(HttpStatusCode.NoContent, "Usertask doesn't exist.");
            }
            catch (Exception e)
            {

                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }

        /// <summary> Changes UserTask result by usertask id. </summary>
        /// <param name="userTaskId">Id of the userTask status to be changed</param>
        /// <param name="newMessage">>New userTask result</param>
        /// <returns></returns>
        /// 
        [HttpPut]
        [Route("api/task/usertask/result")]
        public async Task<HttpResponseMessage> PutNewUserTaskResultAsync(int userTaskId, HttpRequestMessage newMessage)
        {
            try
            {
                var value = newMessage.Content.ReadAsStringAsync().Result;
                if (value.Length >= ValidationRules.MAX_USERTASK_RESULT_LENGTH)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "New Result is too long");
                }
                bool success = await taskService.UpdateUserTaskResultAsync(userTaskId, value);
                if (success)
                {
                    var message = $"Succesfully updated user task with id = {userTaskId} on result {value}";

                    return Request.CreateResponse(HttpStatusCode.OK, "Succesfully updated user task result.");
                }
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Incorrect request syntax or usertask does not exist.");
            }
            catch (Exception e)
            {

                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }

        /// <summary>Returns tasks which name contains string key.</summary>
        /// <param name="key">Key for search.</param>
        [HttpGet]
        [Route("api/task/search")]
        public async Task<HttpResponseMessage> SearchAsync(string key)
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
                    return Request.CreateResponse(HttpStatusCode.NoContent, "There are no tasks by this key");
                }
                return Request.CreateResponse(HttpStatusCode.OK, taskList);
            }
            catch (Exception e)
            {

                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }

        /// <summary>
        /// Returns tasks in plan which names contain string key.
        /// </summary>
        /// <param name="key">Key for search.</param>
        /// <param name="planId">Id of the plan.</param>
        [HttpGet]
        [Route("api/task/searchinplan")]
        public async Task<HttpResponseMessage> SearchInPlanAsync(string key, int planId)
        {
            try
            {
                if (string.IsNullOrEmpty(key))
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Incorrect request syntax.");
                }
                var lines = key.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                IEnumerable<TaskDto> taskList = await taskService.SearchAsync(lines, planId);
                if (taskList == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NoContent, "This plan does not exist.");
                }
                return Request.CreateResponse(HttpStatusCode.OK, taskList);
            }
            catch (Exception e)
            {

                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }

        /// <summary>
        /// Creates new task
        /// </summary>
        /// <param name="newTask">Task object for creation.</param>
        [Authorize(Roles = "Mentor, Admin")]
        [HttpPost]
        [Route("api/task")]
        public HttpResponseMessage Post([FromBody]TaskDto newTask)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                }
                bool success = taskService.CreateTask(newTask);
                if (success)
                {
                    var message = $"Succesfully created task with id = {newTask.Id} by user with id = {newTask.CreatorId}";

                    return Request.CreateResponse(HttpStatusCode.OK, "Task succesfully created");
                }
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Creation error.");
            }
            catch (Exception e)
            {

                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }

        }

        /// <summary>
        /// Creates new task and returns id of the created task.
        /// </summary>
        /// <param name="value"> New plan to be created. </param>
        [Authorize(Roles = "Mentor, Admin")]
        [HttpPost]
        [Route("api/task/return")]
        public async Task<HttpResponseMessage> PostAndReturnIdAsync([FromBody]TaskDto value)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                }
                int? result = await taskService.AddAndGetIdAsync(value);
                if (result != null)
                {
                    var log = $"Succesfully created task {value.Name} with id = {result} by user with id = {value.CreatorId}";
                    return Request.CreateResponse(HttpStatusCode.OK, result);
                }
            }
            catch (Exception e)
            {

                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
            const string message = "Incorrect request syntax.";
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, message);
        }

        /// <summary>
        /// Updates task by Id
        /// </summary>
        /// <param name="taskId">Task Id for update.</param>
        /// <param name="task">Modified task object for update.</param>
        [Authorize(Roles = "Mentor")]
        [HttpPut]
        [Route("api/task/{taskId}")]
        public async Task<HttpResponseMessage> PutAsync(int taskId, [FromBody]TaskDto task)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                }
                bool success = await taskService.UpdateTaskByIdAsync(taskId, task);
                if (success)
                {
                    var message = $"Succesfully updated task with id = {taskId}";

                    return Request.CreateResponse(HttpStatusCode.OK, $"Succesfully updated task id: {taskId}.");
                }
                return Request.CreateErrorResponse(HttpStatusCode.NoContent, "Task doesn't exist.");
            }
            catch (Exception e)
            {

                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
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
        public async Task<HttpResponseMessage> PutAsync(int userTaskId, DateTime proposeEndDate)
        {
            try
            {
                bool success = await taskService.UpdateProposeEndDateAsync(userTaskId, proposeEndDate);
                if (success)
                {
                    var message = $"Succesfully updated usertask with id = {userTaskId}";

                    return Request.CreateResponse(HttpStatusCode.OK, $"Succesfully updated usertask id: {userTaskId}.");
                }
                return Request.CreateErrorResponse(HttpStatusCode.NoContent, "Usertask doesn't exist.");
            }
            catch (Exception e)
            {

                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }

        /// <summary>
        /// Delete proposeEndDate for userTask
        /// </summary>
        /// <param name="userTaskId">UserTask Id for update.</param>
        [Authorize(Roles = "Mentor, Admin")]
        [HttpDelete]
        [Route("api/task/usertask/proposedEndDate")]
        public async Task<HttpResponseMessage> DeleteProposeEndDateAsync(int userTaskId)
        {
            try
            {
                bool success = await taskService.DeleteProposeEndDateAsync(userTaskId);
                if (success)
                {
                    var message = $"Succesfully deleted proposeEndDate for usertask with id = {userTaskId}";

                    return Request.CreateResponse(HttpStatusCode.OK, $"Succesfully deleted proposeEndDate for usertask id: {userTaskId}.");
                }
                return Request.CreateErrorResponse(HttpStatusCode.NoContent, "Usertask doesn't exist.");
            }
            catch (Exception e)
            {

                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }

        /// <summary>
        /// Set new endDate for userTask
        /// </summary>
        /// <param name="userTaskId">UserTask Id for update.</param>
        [Authorize(Roles = "Mentor, Admin")]
        [HttpPut]
        [Route("api/task/usertask/endDate")]
        public async Task<HttpResponseMessage> SetNewEndDateAsync(int userTaskId)
        {
            try
            {
                bool success = await taskService.SetNewEndDateAsync(userTaskId);
                if (success)
                {
                    var message = $"Succesfully changing endDate for usertask with id = {userTaskId}";

                    return Request.CreateResponse(HttpStatusCode.OK, $"Succesfully  changing endDate for usertask id: {userTaskId}.");
                }
                return Request.CreateErrorResponse(HttpStatusCode.NoContent, "Usertask doesn't exist.");
            }
            catch (Exception e)
            {

                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }

        /// <summary>
        /// Deletes task by Id
        /// </summary>
        /// <param name="taskId">Task Id for delete.</param>
        [Authorize(Roles = "Mentor, Admin")]
        [HttpDelete]
        [Route("api/task/{id}")]
        public async Task<HttpResponseMessage> DeleteAsync(int taskId)
        {
            try
            {
                bool success = await taskService.RemoveTaskByIdAsync(taskId);
                if (success)
                {
                    var message = $"Succesfully deleted task with id = {taskId}";

                    return Request.CreateResponse(HttpStatusCode.OK, $"Succesfully deleted task id: {taskId}.");
                }
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, $"No task with id: {taskId} or cannot be deleted because of dependency conflict.");
            }
            catch (Exception e)
            {

                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
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
