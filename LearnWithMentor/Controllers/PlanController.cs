using LearnWithMentorDTO;
using LearnWithMentorBLL.Interfaces;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.WebSockets.Internal;

namespace LearnWithMentor.Controllers
{
    [Authorize]
    public class PlanController : ControllerBase
    {
        private readonly IPlanService planService;
        private readonly ITaskService taskService;
        private readonly ITraceWriter tracer;

        /// <summary>
        /// Creates new instance of controller.
        /// </summary>
        public PlanController(IPlanService planService, ITaskService taskService)
        {
            this.planService = planService;
            this.taskService = taskService;
            this.tracer = tracer;
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
                //const string errorMessage = "No plans in database.";
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
                //const string message = "Plan does not exist in database.";
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
                //const string message = "Plan or Group does not exist in database.";
                return NoContent();
            }
            return Ok(info);
        }

        /// <summary>
        /// Returns tasks for concrete plan grouped by sections.
        /// </summary>
        /// <param name="id"> Id of the plan. </param>
        [HttpGet]
        [Route("api/plan/{id}/sections")]
        public async Task<ActionResult> GetTasksForPlanAsync(int id)
        {
            List<SectionDto> sections = await planService.GetTasksForPlanAsync(id);
            if (sections == null)
            {
                //const string message = "Plan does not exist in database.";
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
                //const string errorMessage = "No plans in database.";
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
            List<TaskDto> dtosList = await planService.GetAllTasksAsync(planId);
            if (dtosList == null || dtosList.Count == 0)
            {
                //const string message = "Plan does not contain any task.";
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
                //const string message = "Plan does not contain any plantask.";
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
        public async Task<ActionResult> PostAsync([FromBody]PlanDto value)
        {
            try
            {
                var success = await planService.AddAsync(value);
                if (success)
                {
                    var okMessage = $"Succesfully created plan: {value.Name}";
                    return Ok(okMessage);
                }
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }
            const string message = "Incorrect request syntax.";
            return BadRequest(message);
        }

        /// <summary>
        /// Creates new plan and returns id of the created plan.
        /// </summary>
        /// <param name="value"> New plan to be created. </param>
        [Authorize(Roles = "Mentor")]
        [HttpPost]
        [Route("api/plan/return")]
        public async Task<ActionResult> PostAndReturnIdAsync([FromBody]PlanDto value)
        {
            try
            {
                if (!ModelState.IsValid)
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
                return StatusCode(500);
            }
            const string message = "Incorrect request syntax.";
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
        public async Task<ActionResult> PutAsync(int id, [FromBody]PlanDto value)
        {
            try
            {
                var success = await planService.UpdateByIdAsync(value, id);
                if (success)
                {
                    const string okMessage = "Succesfully updated plan.";
                    return Ok(okMessage);
                }
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }
            const string message = "Incorrect request syntax or plan does not exist.";
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
                    return Ok($"Succesfully added task to plan ({id}).");
                }
                return BadRequest("Incorrect request syntax or task or plan does not exist.");
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }
        }

        ///// <summary>
        ///// Sets image for plan by plan id.
        ///// </summary>
        ///// <param name="id"> Id of the plan. </param>
        //[Authorize(Roles = "Mentor")]
        //[HttpPost]
        //[Route("api/plan/{id}/image")]
        //public async Task<ActionResult> PostImageAsync(int id)
        //{
        //    if (!await planService.ContainsId(id))
        //    {
        //        //const string errorMessage = "No plan with this id in database.";
        //        return NoContent();
        //    }
        //    if (HttpContext.Current.Request.Files.Count != 1)
        //    {
        //        const string errorMessage = "Only one image can be sent.";
        //        return BadRequest(errorMessage);
        //    }
        //    try
        //    {
        //        var postedFile = HttpContext.Current.Request.Files[0];
        //        if (postedFile.ContentLength > 0)
        //        {
        //            var allowedFileExtensions = new List<string>(Constants.ImageRestrictions.Extensions);
        //            var extension = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.')).ToLower();
        //            if (!allowedFileExtensions.Contains(extension))
        //            {
        //                const string errorMessage = "Types allowed only .jpeg .jpg .png";
        //                return BadRequest(errorMessage);
        //            }
        //            const int maxContentLength = Constants.ImageRestrictions.MaxSize;
        //            if (postedFile.ContentLength > maxContentLength)
        //            {
        //                const string errorMessage = "Please Upload a file upto 1 mb.";
        //                return BadRequest(errorMessage);
        //            }
        //            byte[] imageData;
        //            using (var binaryReader = new BinaryReader(postedFile.InputStream))
        //            {
        //                imageData = binaryReader.ReadBytes(postedFile.ContentLength);
        //            }
        //            await planService.SetImageAsync(id, imageData, postedFile.FileName);
        //            const string okMessage = "Successfully created image.";
        //            return Ok(okMessage);
        //        }
        //        //const string emptyImageMessage = "Empty image.";
        //        return StatusCode(304);
        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(e);
        //    }
        //}

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
                    //const string errorMessage = "No plan with this id in database.";
                    return NoContent();
                }
                var dto = await planService.GetImageAsync(id);
                if (dto == null)
                {
                    //const string message = "No image for this plan in database.";
                    return NoContent();
                }
                return Ok(dto);
            }
            catch (Exception e)
            {
                return StatusCode(500);
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
                //const string message = "No plans found.";
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
                    var message = $"Succesfully deleted plan with id = {id}";
                    return Ok($"Succesfully deleted plan id: {id}.");
                }
                return BadRequest($"No plan with id: {id} or cannot be deleted because of dependency conflict.");
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Releases memory
        /// </summary>
        protected void Dispose(bool disposing)
        {
            planService.Dispose();
            taskService.Dispose();
            //base.Dispose(disposing);
        }
    }
}
