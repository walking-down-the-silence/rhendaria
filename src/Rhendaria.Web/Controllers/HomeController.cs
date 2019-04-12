using Microsoft.AspNetCore.Mvc;
using Orleans;
using Rhendaria.Web.Extensions;
using Rhendaria.Web.Models;
using Rhendaria.Web.Services;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Rhendaria.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IClusterClient _client;
        private readonly PlayerMovementService _movementService;

        public HomeController(
            IClusterClient client,
            PlayerMovementService movementService)
        {
            _client = client;
            _movementService = movementService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> JoinGame([Bind] JoinGameViewModel viewModel)
        {
            await _movementService.SpawnPlayer(viewModel.Username);
            var routeValues = new { nickname = viewModel.Username };
            return RedirectToAction(nameof(GameController.Index), this.NameOf<GameController>(), routeValues);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [HttpGet]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Privacy()
        {
            throw new System.NotImplementedException();
        }
    }
}
