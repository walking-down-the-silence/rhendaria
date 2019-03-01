using Microsoft.AspNetCore.Mvc;
using Orleans;
using Rhendaria.Abstraction.Actors;
using Rhendaria.Web.Commands;
using Rhendaria.Web.Models;
using Rhendaria.Web.Services;
using System.Threading.Tasks;

namespace Rhendaria.Web.Controllers
{
    [Route("api/player")]
    [ApiController]
    public class PlayerController : ControllerBase
    {
        private readonly IClusterClient _client;
        private readonly PlayerMovementService _movementService;

        public PlayerController(IClusterClient client, PlayerMovementService movementService)
        {
            _client = client;
            _movementService = movementService;
        }

        [HttpGet("{nickname}")]
        public async Task<IActionResult> GetGameView(string nickname)
        {
            if (string.IsNullOrWhiteSpace(nickname))
            {
                return BadRequest("Username cannot be null or empty.");
            }

            var playerActor = _client.GetGrain<IPlayerActor>(nickname);
            var state = await playerActor.GetState();

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
                        Position = new VectorModel { X = state.Position.X, Y = state.Position.Y }
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

            var playerPosition = await _movementService.MovePlayer(nickname, command.Direction);
            return Ok(playerPosition);
        }
    }
}