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

namespace LearnWithMentor.Controllers
{
    [ApiController]
    public class PlanController : ControllerBase
    {
        private readonly IPlanService planService;
        private readonly ITaskService taskService;

        public PlanController(IPlanService planService, ITaskService taskService)
        {
            this.planService = planService;
            this.taskService = taskService;
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
                const string errorMessage = "No plans in database.";
                return NotFound(errorMessage);
            }
            return Ok(dtoList);
        }

        /// <summary>
        /// Gets plan by id.
        /// </summary>
        /// <param name="id"> Id of the plan. </param>
        [HttpGet]
        [Route("api/plan/{id}")]
        public async Task<ActionResult> GetAsync(int id)
        {
            var plan = await planService.GetAsync(id);
            if (plan == null)
            {
                const string message = "Plan does not exist in database.";
                return NotFound(message);
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
                const string message = "Plan or Group does not exist in database.";
                return NotFound(message);
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
                const string message = "Plan does not exist in database.";
                return NotFound(message);
            }
            return Ok(sections);
        }

        /// <summary>
        /// Gets some number of plans on page. 
        /// </summary>
        /// <param name="prevAmount"> Previous amount to start with. </param>
        /// <param name="amount"> Amount of plans to be returned. </param>
        [HttpGet]
        [Route("api/plan/some")]
        public IActionResult GetSome(int prevAmount, int amount)
        {
            var dtoList = planService.GetSomeAmount(prevAmount, amount);
            if (dtoList == null || dtoList.Count == 0)
            {
                const string errorMessage = "No plans in database.";
                return NotFound(errorMessage);
            }
            return Ok(dtoList);
        }
    }

}
