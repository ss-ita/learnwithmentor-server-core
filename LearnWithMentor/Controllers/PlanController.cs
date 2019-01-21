using System;
using System.Collections.Generic;
using LearnWithMentorDTO;
using LearnWithMentorBLL.Interfaces;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace LearnWithMentor.Controllers
{
    /// <summary>
    /// Controller for plans.
    /// </summary>
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class PlanController : Controller
    {
        private readonly IPlanService planService;
        private readonly ITaskService taskService;
        private readonly IHttpContextAccessor _accessor;

        /// <summary>
        /// Creates new instance of controller.
        /// </summary>
        public PlanController(IPlanService planService, ITaskService taskService, IHttpContextAccessor accessor)
        {
            this.planService = planService;
            this.taskService = taskService;
            _accessor = accessor;
        }

        /// <summary>
        /// Returns all plans in database.
        /// </summary>
        [AllowAnonymous]
        [HttpGet]
        [Route("api/plan")]
        public async Task<ActionResult> Get()
        {
            var dtoList = await planService.GetAll();
            if (dtoList == null || dtoList.Count == 0)
            {
                return NoContent();
            }

            return Ok(dtoList);
        }

        /// <summary>
        /// Gets plan by id.
        /// </summary>
        /// <param name="id"> Id of the plan. </param>
        [AllowAnonymous]
        [HttpGet]
        [Route("api/plan/{id}")]
        public async Task<ActionResult> GetAsync(int id)
        {
            var plan = await planService.GetAsync(id);
            if (plan == null)
            {
                return NoContent();
            }
            return Ok(plan);
        }

        /// <summary>
        /// Gets plan name and group name  by groupid and planid.
        /// </summary>
        /// <param name="groupid"> Id of the group. </param>
        ///  <param name="planid"> Id of the plan. </param>
        [HttpGet]
        [Route("api/plan/{planid}/group/{groupid}")]
        public async Task<ActionResult> GetInfoAsync(int groupid, int planid)
        {
            var info = await planService.GetInfoAsync(groupid, planid);
            if (info == null)
            {
                return NoContent();
            }
            return new JsonResult(info);
        }

        /// <summary>
        /// Returns tasks for concrete plan grouped by sections.
        /// </summary>
        /// <param name="id"> Id of the plan. </param>
        [HttpGet]
        [Route("api/plan/{id}/sections")]
        public async Task<ActionResult> GetTasksForPlanAsync(int id)

        {
            List<SectionDTO> sections = await planService.GetTasksForPlanAsync(id);
            if (sections == null)
            {
                return NoContent();
            }
            return Ok(sections);
        }

        /// <summary>
        /// Gets some number of plans on page. 
        /// </summary>
        /// <param name="prevAmount"> Previous amount to start with. </param>
        /// <param name="amount"> Amount of plans to be returned. </param>
        [AllowAnonymous]
        [HttpGet]
        [Route("api/plan/some")]
        public ActionResult GetSome(int prevAmount, int amount)
        {
            var dtoList = planService.GetSomeAmount(prevAmount, amount);
            if (dtoList == null || dtoList.Count == 0)
            {
                return NoContent();
            }
            return Ok(dtoList);
        }

        /// <summary>
        /// Gets all tasks assigned to plan.
        /// </summary>
        /// <param name="planId"> Id of plan. </param>
        [AllowAnonymous]
        [HttpGet]
        [Route("api/plan/{planId}/tasks")]
        public async Task<ActionResult> GetAllTasksAsync(int planId)
        {
            List<TaskDTO> dtosList = await planService.GetAllTasksAsync(planId);
            if (dtosList == null || dtosList.Count == 0)
            {
                return NoContent();
            }
            return Ok(dtosList);
        }

        /// <summary>
        /// Gets all Plantask ids of concrete plan.
        /// </summary>
        /// <param name="planId"> Id of plan. </param>
        [HttpGet]
        [Route("api/plan/{planId}/plantaskids")]
        public async Task<ActionResult> GetAllPlanTaskIdsAsync(int planId)
        {
            var idsList = await planService.GetAllPlanTaskidsAsync(planId);
            if (idsList == null || idsList.Count == 0)
            {
                return NoContent();
            }
            return Ok(idsList);
        }

        /// <summary>
        /// Creates new plan.
        /// </summary>
        /// <param name="value"> New plan to be created. </param>
        [Authorize(Roles = "Mentor")]
        [HttpPost]
        [Route("api/plan")]
        public async Task<ActionResult> PostAsync([FromBody]PlanDTO value)
        {
            try
            {
                var success = await planService.AddAsync(value);
                if (success)
                {
                    string okMessage = $"Succesfully created plan: {value.Name}";
                    return Ok(okMessage);
                }
            }
            catch (Exception e)
            {
                return BadRequest();
            }
            string message = "Incorrect request syntax.";
            return BadRequest(message);
        }

        /// <summary>
        /// Creates new plan and returns id of the created plan.
        /// </summary>
        /// <param name="value"> New plan to be created. </param>
        [Authorize(Roles = "Mentor")]
        [HttpPost]
        [Route("api/plan/return")]
        public async Task<ActionResult> PostAndReturnIdAsync([FromBody]PlanDTO value)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                int? result = await planService.AddAndGetIdAsync(value);
                if (result != null)
                {
                    return Ok(result);
                }
            }
            catch (Exception e)
            {
                return BadRequest();
            }
            string message = "Incorrect request syntax.";
            return BadRequest(message);
        }

        /// <summary>
        /// Updates existing plan.
        /// </summary>
        /// <param name="id"> Id of plan to be updated. </param>
        /// <param name="value"> Plan info to be updated. </param>
        [Authorize(Roles = "Mentor, Admin")]
        [HttpPut]
        [Route("api/plan/{id}")]
        public async Task<ActionResult> PutAsync(int id, [FromBody]PlanDTO value)
        {
            try
            {
                var success = await planService.UpdateByIdAsync(value, id);
                if (success)
                {
                    string okMessage = "Succesfully updated plan.";
                    return Ok();
                }
            }
            catch (Exception e)
            {
                return BadRequest();
            }
            string message = "Incorrect request syntax or plan does not exist.";
            return BadRequest(message);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="taskId"></param>
        /// <param name="sectionId"></param>
        /// <param name="priority"></param>
        /// <returns></returns>            
        [Authorize(Roles = "Mentor, Admin")]
        [HttpPut]
        [Route("api/plan/{id}/task/{taskId}")]
        public async Task<ActionResult> PutTaskToPlanAsync(int id, int taskId, string sectionId, string priority)
        {
            try
            {
                int? section;
                int? priorityNew;
                if (string.IsNullOrEmpty(sectionId))
                {
                    section = null;
                }
                else
                {
                    section = int.Parse(sectionId);
                }
                if (string.IsNullOrEmpty(priority))
                {
                    priorityNew = null;
                }
                else
                {
                    priorityNew = int.Parse(priority);
                }
                bool success = await planService.AddTaskToPlanAsync(id, taskId, section, priorityNew);
                if (success)
                {
                    string okMessage = $"Succesfully added task to plan ({id}).";
                    return Ok(okMessage);
                }
                string message = "Incorrect request syntax or task or plan does not exist.";
                return BadRequest(message);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

        ///// <summary>
        ///// Sets image for plan by plan id.
        ///// </summary>
        ///// <param name="id"> Id of the plan. </param>
        [Authorize(Roles = "Mentor")]
        [HttpPost]
        [Route("api/plan/{id}/image")]
        public async Task<ActionResult> PostImageAsync(int id)
        {
            if (!await planService.ContainsId(id))
            {
                return NoContent();
            }
            if (_accessor.HttpContext.Request.Form.Files.Count != 1)
            {
                string errorMessage = "Only one image can be sent.";
                return BadRequest(errorMessage);
            }
            try
            {
                var postedFile = _accessor.HttpContext.Request.Form.Files[0];
                if (postedFile.Length > 0)
                {
                    var allowedFileExtensions = new List<string>(Constants.ImageRestrictions.Extensions);
                    var extension = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.')).ToLower();
                    if (!allowedFileExtensions.Contains(extension))
                    {
                        string errorMessage = "Types allowed only .jpeg .jpg .png";
                        return BadRequest(errorMessage);
                    }
                    const int maxContentLength = Constants.ImageRestrictions.MaxSize;
                    if (postedFile.Length > maxContentLength)
                    {
                        string errorMessage = "Please Upload a file upto 1 mb.";
                        return BadRequest(errorMessage);
                    }
                    byte[] imageData;
                    using (var binaryReader = new BinaryReader(postedFile.OpenReadStream()))
                    {
                        imageData = binaryReader.ReadBytes(Convert.ToInt32(postedFile.Length));
                    }
                    await planService.SetImageAsync(id, imageData, postedFile.FileName);
                    string okMessage = "Successfully created image.";
                    return Ok(okMessage);
                }
                return StatusCode(304);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Returns image of concrete plan form database.
        /// </summary>
        /// <param name="id"> Id of the plan. </param>
        [AllowAnonymous]
        [HttpGet]
        [Route("api/plan/{id}/image")]
        public async Task<ActionResult> GetImageAsync(int id)
        {
            try
            {
                if (!await planService.ContainsId(id))
                {
                    return NoContent();
                }
                var dto = await planService.GetImageAsync(id);
                if (dto == null)
                {
                    return NoContent();
                }
                return Ok(dto);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Searches plans that match q string
        /// </summary>
        /// <param name="q">Match string</param>
        [HttpGet]
        [Route("api/plan/search")]
        public async Task<ActionResult> Search(string q)
        {
            if (string.IsNullOrEmpty(q))
            {
                return await Get();
            }
            var lines = q.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var dto = planService.Search(lines);
            if (dto == null || dto.Count == 0)
            {
                return NoContent();
            }
            return Ok(dto);
        }

        /// <summary>
        /// Deletes plan by Id
        /// </summary>
        /// <param name="id">Plan Id for delete.</param>
        [Authorize(Roles = "Mentor, Admin")]
        [HttpDelete]
        [Route("api/plan/{id}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            try
            {
                bool success = await planService.RemovePlanByIdAsync(id);
                if (success)
                {
                    string okMessage = $"Succesfully deleted plan with id = {id}";
                    return Ok();
                }
                string message = $"No plan with id: {id} or cannot be deleted because of dependency conflict.";
                return BadRequest(message);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Releases memory
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            planService.Dispose();
            taskService.Dispose();
            base.Dispose(disposing);
        }
    }
}
