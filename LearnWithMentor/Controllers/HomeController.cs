using Microsoft.AspNetCore.Mvc;

namespace LearnWithMentor.Controllers
{
    [Route("api/[controller]")]
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
    }
}
