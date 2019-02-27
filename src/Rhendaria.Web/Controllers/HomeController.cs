using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Orleans;
using Rhendaria.Abstraction.Actors;
using Rhendaria.Web.Extensions;
using Rhendaria.Web.Models;

namespace Rhendaria.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IClusterClient _client;

        public HomeController(IClusterClient client)
        {
            _client = client;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> JoinGame([Bind] JoinGameViewModel viewModel)
        {
            string nickname = await _client.GetGrain<IPlayerActor>(viewModel.Username).GetUsername();
            return RedirectToAction(nameof(GameController.Index), this.NameOf<GameController>());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [HttpGet]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
