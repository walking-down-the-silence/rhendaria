using Microsoft.AspNetCore.Mvc;

namespace Rhendaria.Web.Controllers
{
    public class GameController : Controller
    {
        [HttpGet]
        public IActionResult Index([FromQuery] string nickname)
        {
            return View();
        }
    }
}