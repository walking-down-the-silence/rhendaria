using Microsoft.AspNetCore.Mvc;
using Orleans;
using Rhendaria.Abstraction.Actors;
using Rhendaria.Web.Extensions;
using Rhendaria.Web.Models;
using System.Diagnostics;

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
        public IActionResult JoinGame([Bind] JoinGameViewModel viewModel)
        {
            _client.GetGrain<IPlayerActor>(viewModel.Username).GetPrimaryKeyString();
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
