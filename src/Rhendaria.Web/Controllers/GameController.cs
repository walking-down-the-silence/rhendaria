using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Orleans;
using Rhendaria.Abstraction;
using Rhendaria.Abstraction.Actors;
using Rhendaria.Web.Extensions;
using Rhendaria.Web.Models;

namespace Rhendaria.Web.Controllers
{
    public class GameController : Controller
    {
        private readonly IClusterClient _client;

        public GameController(IClusterClient client)
        {
            _client = client;
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                return RedirectToAction(nameof(HomeController.Error), this.NameOf<HomeController>());
            }

            Vector2D position = await _client.GetGrain<IPlayerActor>(username).GetPosition();

            GameZoneViewModel gameZoneViewModel = new GameZoneViewModel
            {
                Player = new SpriteStateViewModel
                {
                    Nickname = username,
                    PositionX = position.Left,
                    PositionY = position.Top
                },
                Zone = new ZoneViewModel
                {
                    TopLeftX = 12,
                    TopLeftY = 8,
                    BottomRightX = 24,
                    BottomRightY = 16
                }
            };

            return View(gameZoneViewModel);
        }
    }
}