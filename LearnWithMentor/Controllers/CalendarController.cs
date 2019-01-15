using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace LearnWithMentor.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [EnableCors(Constants.Cors.policyName)]
    public class CalendarController : Controller
    {
        [Route("api/calendar/{currentEvent}")]
        [HttpGet]
        public async Task<IActionResult> SendCalendarEvent(string message)
        {
            try
            {
                var user = message;
                if (user != null)
                {
                    Console.Write("Hello");
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