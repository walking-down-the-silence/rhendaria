using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Rhendaria.Web.Services;

namespace Rhendaria.Web.Controllers
{
    public class GameController : Controller
    {
        private readonly PlayerMovementService _movementService;

        public GameController(PlayerMovementService movementService)
        {
            _movementService = movementService;
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] string nickname)
        {
            await _movementService.SpawnPlayer(nickname);
            return View();
        }
    }
}