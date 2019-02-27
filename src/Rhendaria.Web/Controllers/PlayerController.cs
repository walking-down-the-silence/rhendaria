using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Orleans;
using Rhendaria.Abstraction;
using Rhendaria.Abstraction.Actors;
using Rhendaria.Web.Commands;
using Rhendaria.Web.Hubs;
using Rhendaria.Web.Models;
using System.Threading.Tasks;

namespace Rhendaria.Web.Controllers
{
    [Route("api/player")]
    [ApiController]
    public class PlayerController : ControllerBase
    {
        private readonly IClusterClient _client;
        private readonly IHubContext<GameHub> _hubContext;

        public PlayerController(IClusterClient client, IHubContext<GameHub> hubContext)
        {
            _client = client;
            _hubContext = hubContext;
        }

        [HttpGet("{nickname}")]
        public async Task<IActionResult> GetGameView(string nickname)
        {
            if (string.IsNullOrWhiteSpace(nickname))
            {
                return BadRequest("Username cannot be null or empty.");
            }

            var playerActor = _client.GetGrain<IPlayerActor>(nickname);
            var zoneActor = _client.GetGrain<IZoneActor>("zone");

            var position = await playerActor.GetPosition();
            //var zone = await zoneActor.

            GameModel gameZoneViewModel = new GameModel
            {
                Player = new SpriteModel
                {
                    Nickname = nickname,
                    Position = new VectorModel { X = position.Left, Y = position.Top }
                },
                Zone = new ZoneModel
                {
                    Box = new BoxModel
                    {
                        TopLeft = new VectorModel { X = 12, Y = 8 },
                        BottomRight = new VectorModel { X = 24, Y = 16 }
                    }
                }
            };

            return Ok(gameZoneViewModel);
        }

        [HttpPost("{nickname}/move")]
        public async Task<IActionResult> MovePlayer(string nickname, [FromBody] MovementCommand command)
        {
            if (string.IsNullOrWhiteSpace(nickname))
            {
                return BadRequest("Username cannot be null or empty.");
            }

            var player = _client.GetGrain<IPlayerActor>(nickname);
            var zone = _client.GetGrain<IZoneActor>("zone");

            Vector2D position = await player.Move(command.Direction);
            await zone.RoutePlayerMovement(player);
            await _hubContext.Clients.All.SendAsync("UpdatePosition", nickname, position);

            return Ok(position);
        }
    }
}