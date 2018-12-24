using System;
using LearnWithMentorDTO;
using LearnWithMentorBLL.Interfaces;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace LearnWithMentor.Controllers
{
    /// <summary>
    /// Controller for groups.
    /// </summary>
    [Authorize(AuthenticationSchemes = "Bearer")]
    [EnableCors(Constants.Cors.policyName)]


    public class GroupController : Controller
    {
        private readonly IGroupService groupService;
        private readonly IUserService userService;
        private readonly IUserIdentityService userIdentityService;

        /// <summary>
        /// Creates new instance of controller.
        /// </summary>
        public GroupController(IGroupService groupService, IUserService userService, IUserIdentityService userIdentityService)
        {
            this.userService = userService;
            this.groupService = groupService;
            this.userIdentityService = userIdentityService;
        }

        // GET api/<controller>
        /// <summary>
        /// Returns group by mentor Id "api/group/mentor/{id}"
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/group/mentor/{id}")]
        public async Task<ActionResult> GetByMentorAsync(int id)
        {
            IEnumerable<GroupDTO> allGroups = await groupService.GetGroupsByMentorAsync(id);
            if (allGroups != null)
            {
                return Ok(allGroups);
            }
            var notFoundMessage = "No groups for the mentor in database. (mentorId = {id})";
            return NotFound(notFoundMessage);
        }

        /// <summary>
        /// Returns group by Id "api/group/{id}"
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/group/{id}")]
        public async Task<ActionResult> GetByIdAsync(int id)
        {
            GroupDTO group = await groupService.GetGroupByIdAsync(id);
            if (group != null)
            {
                return Ok(group);
            }
            var notFoundMessage = "There isn't group with id = {id}";
            return NotFound(notFoundMessage);
        }

        /// <summary>
        /// Returns plans for specific group by group Id "api/group/{id}/plans"
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/group/{id}/plans")]
        public async Task<ActionResult> GetPlansAsync(int id)
        {
            try
            {
                IEnumerable<PlanDTO> group = await groupService.GetPlansAsync(id);
                if (group != Enumerable.Empty<PlanDTO>())
                {
                    return Ok(group);
                }
                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        /// <summary>
        /// Returns users that belong to group by group Id "api/group/{id}/users"
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/group/{id}/users")]
        public async Task<ActionResult> GetUsersAsync(int id)
        {
            try
            {
                IEnumerable<UserIdentityDTO> group = await groupService.GetUsersAsync(id);
                if (group != null)
                {
                    return Ok(group);
                }
                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        /// <summary>
        /// Returns users that belong to group by group Id "api/group/{id}/users"
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/group/{id}/userimages")]
        public async Task<ActionResult> GetUsersWithImageAsync(int id)
        {
            try
            {
                var group = await groupService.GetUsersWithImageAsync(id);
                if (group != null)
                {
                    return Ok(group);
                }
                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        /// <summary>
        /// Returns users that is not belong to group by group Id "api/group/{id}/users"
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        [Authorize(Roles = "Mentor")]
        [HttpGet]
        [Route("api/group/{groupId}/users/notingroup")]
        public async Task<ActionResult> GetUsersNotInCurrentGroupAsync(int groupId)
        {
            IEnumerable<UserIdentityDTO> group = await groupService.GetUsersNotInGroupAsync(groupId);
            if (group != null)
            {
                return Ok(group);
            }
            var notFoundMessage = "There isn't users outside of the group id = {groupId}";
            return NotFound(notFoundMessage);
        }

        /// <summary>
        /// Returns all plans not used in current group.
        /// </summary>
        [Authorize(Roles = "Mentor")]
        [HttpGet]
        [Route("api/plan/notingroup/{groupId}")]
        public async Task<ActionResult> GetPlansNotUsedInCurrentGroupAsync(int groupId)
        {
            var notUsedPlans = await groupService.GetPlansNotUsedInGroupAsync(groupId);
            if (notUsedPlans != null)
            {
                return Ok(notUsedPlans);
            }
            return NoContent();
        }

        /// <summary>
        /// Create new group
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        [Authorize(Roles = "Mentor")]
        [HttpPost]
        [Route("api/group")]
        public async Task<ActionResult> PostAsync([FromBody]GroupDTO group)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var success = groupService.AddGroupAsync(group);
                if (await success)
                {
                    var okMessage = "Succesfully created group: {group.Name}.";
                    return Ok(okMessage);
                }
                var badRequestMessage = "Group with this name already exists";
                return BadRequest(badRequestMessage);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        /// <summary>
        /// Add users array to group by group id. You have to pass users Id as int[] in body "api/group/{id}/user"
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [Authorize(Roles = "Mentor")]
        [HttpPut]
        [Route("api/group/{id}/user")]
        public async Task<ActionResult> PutUsersToGroupAsync(int id, [FromBody] int[] userId)
        {
            try
            {
                var currentUserId = userIdentityService.GetUserId();
                int? mentorId = await groupService.GetMentorIdByGroupAsync(id);
                if (mentorId != currentUserId)
                {
                    return Unauthorized();
                }
                bool success = await groupService.AddUsersToGroupAsync(userId, id);
                if (success)
                {
                    var okMessage = "Succesfully added users to group ({id}).";
                    return Ok(okMessage);
                }
                var badRequestMessage = "Incorrect request syntax or user or group does not exist.";
                return BadRequest(badRequestMessage);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        /// <summary>
        /// Add plans array to group by groupId. You have to pass plans Id as int[] in body "api/group/{id}/plan"
        /// </summary>
        /// <param name="id"></param>
        /// <param name="planId"></param>
        /// <returns></returns>
        [Authorize(Roles = "Mentor")]
        [HttpPut]
        [Route("api/group/{id}/plan")]
        public async Task<ActionResult> PutPlansToGroupAsync(int id, [FromBody] int[] planId)
        {
            try
            {
                var userId = userIdentityService.GetUserId();
                int? mentorId = await groupService.GetMentorIdByGroupAsync(id);
                if (mentorId != userId)
                {
                    return Unauthorized();
                }
                bool success = await groupService.AddPlansToGroupAsync(planId, id);
                if (success)
                {
                    var okMessage = "Succesfully added plans to group ({id}).";
                    return Ok(okMessage);
                }
                var badRequestMessage = "Incorrect request syntax or plan or group does not exist.";
                return BadRequest(badRequestMessage);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        /// <summary>
        /// Returns plans that is no used in current group and which names contain string key.
        /// </summary>
        /// <param name="searchKey">Key for search.</param>
        /// <param name="groupId">Id of the plan.</param>
        [HttpGet]
        [Route("api/group/searchinNotUsedPlan")]
        public async Task<ActionResult> SearchPlansNotUsedInCurrentGroupAsync(string searchKey, int groupId)
        {
            try
            {
                if (searchKey == null)
                {
                    return await GetPlansNotUsedInCurrentGroupAsync(groupId);
                }
                var lines = searchKey.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                var plansList = await groupService.SearchPlansNotUsedInGroupAsync(lines, groupId);
                if (plansList == null)
                {
                    return NoContent();
                }
                return Ok(plansList);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        /// <summary>
        /// Returns users that is not involved in current group and which names contain string key.
        /// </summary>
        /// <param name="searchKey">Key for search.</param>
        /// <param name="groupId">Id of the plan.</param>
        [HttpGet]
        [Route("api/group/searchinNotInvolvedUsers")]
        public async Task<ActionResult> SearchUsersNotUsedInCurrentGroupAsync(string searchKey, int groupId)
        {
            try
            {
                if (searchKey == null)
                {
                    return await GetUsersNotInCurrentGroupAsync(groupId);
                }
                var lines = searchKey.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                var usersList = groupService.SearchUserNotInGroupAsync(lines, groupId);
                if (usersList == null)
                {
                    return NoContent();
                }
                return Ok(usersList);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        /// <summary>
        /// Removes user from current group.
        /// </summary>
        /// <param name="groupId">Group ID where user should be removed.</param>
        /// <param name="userToRemoveId">Id of the user to remove.</param>
        [Authorize(Roles = "Mentor")]
        [HttpDelete]
        [Route("api/group/removeUserFromGroup")]
        public async Task<ActionResult> RemoveUserFromCurrentGroupAsync(int groupId, int userToRemoveId)
        {
            try
            {
                var id = userIdentityService.GetUserId();
                int? mentorId = await groupService.GetMentorIdByGroupAsync(groupId);
                if (mentorId != id)
                {
                    return Unauthorized();
                }
                bool successfullyRemoved = await groupService.RemoveUserFromGroupAsync(groupId, userToRemoveId);
                if (successfullyRemoved)
                {
                    var okMessage = "User succesfully removed.";
                    return Ok(okMessage);
                }
                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }

        }

        /// <summary>
        /// Removes plan from current group.
        /// </summary>
        /// <param name="groupId">Group ID where user should be removed.</param>
        /// <param name="planToRemoveId">Id of the plan to remove.</param>
        [Authorize(Roles = "Mentor")]
        [HttpDelete]
        [Route("api/group/removePlanFromGroup")]
        public async Task<ActionResult> RemovePlanFromCurrentGroupAsync(int groupId, int planToRemoveId)
        {
            try
            {
                bool successfullyRemoved = await groupService.RemovePlanFromGroupAsync(groupId, planToRemoveId);
                if (successfullyRemoved)
                {
                    var okMessage = "Plan succesfully removed from group.";
                    return Ok(okMessage);
                }
                var badRequestMessage = "Incorrect request syntax: plan or group does not exist.";
                return BadRequest(badRequestMessage);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }

        }

        /// <summary>
        /// If user: strudent - returns its learning groups, if mentor - returns mentored groups, if admin - returns all groups."
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        [Route("api/group/mygroups")]
        public async Task<ActionResult> GetUserGroupsAsync()
        {
            try
            {
                var userId = userIdentityService.GetUserId();
                if (!(await userService.ContainsIdAsync(userId)))
                {
                    return NoContent();
                }
                if (await groupService.GroupsCountAsync() == 0)
                {
                    return NoContent();
                }
                var groups = await groupService.GetUserGroupsAsync(userId);
                if (groups == null)
                {
                    var badRequestMessage = "There are no groups for this user";
                    return BadRequest(badRequestMessage);
                }
                return Ok(groups);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        /// <summary>
        /// Releases memory
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            groupService.Dispose();
            base.Dispose(disposing);
        }
    }
}
