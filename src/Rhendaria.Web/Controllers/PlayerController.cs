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

            var gameZoneViewModel = new GameModel
            {
                Player = new PlayerModel
                {
                    Nickname = nickname
                },
                Zone = new ZoneModel
                {
                    Box = new BoxModel
                    {
                        TopLeft = new VectorModel { X = 1280, Y = 720 },
                        BottomRight = new VectorModel { X = 2560, Y = 1440 }
                    }
                },
                Sprites = new[]
                {
                    new SpriteModel
                    {
                        Nickname = nickname,
                        Color = 0x3366FF,
                        Position = new VectorModel { X = position.Left, Y = position.Top }
                    },
                    new SpriteModel
                    {
                        Nickname = "sickranchez",
                        Color = 0x339966,
                        Position = new VectorModel { X = 1650, Y = 1100 }
                    },
                    new SpriteModel
                    {
                        Nickname = "alienware51",
                        Color = 0xFFFF99,
                        Position = new VectorModel { X = 1800, Y = 1150 }
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

            var playerPosition = new VectorModel { X = position.Left, Y = position.Top };
            await _hubContext.Clients.All.SendAsync("UpdatePosition", nickname, playerPosition);

            return Ok(playerPosition);
        }
    }
}