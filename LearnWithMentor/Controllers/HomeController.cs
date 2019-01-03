using Microsoft.AspNetCore.Mvc;

namespace LearnWithMentor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : Controller
    {
        // GET api/values
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
    }
}
