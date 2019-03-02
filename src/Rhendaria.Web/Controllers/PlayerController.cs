using Microsoft.AspNetCore.Mvc;
using Orleans;
using Rhendaria.Abstraction.Actors;
using Rhendaria.Abstraction.Services;
using Rhendaria.Web.Commands;
using Rhendaria.Web.Models;
using Rhendaria.Web.Services;
using System.Linq;
using System.Threading.Tasks;

namespace Rhendaria.Web.Controllers
{
    [Route("api/player")]
    [ApiController]
    public class PlayerController : ControllerBase
    {
        private readonly IClusterClient _client;
        private readonly IRoutingService _routingService;
        private readonly PlayerMovementService _movementService;

        public PlayerController(
            IClusterClient client,
            IRoutingService routingService,
            PlayerMovementService movementService)
        {
            _client = client;
            _routingService = routingService;
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
            var playerInfo = await playerActor.GetState();

            string zoneId = _routingService.GetZoneId(playerInfo.Position);
            var zoneActor = _client.GetGrain<IZoneActor>(zoneId);
            var zoneInfo = await zoneActor.GetPlayers();
            var players = zoneInfo.Players.Select(player => player.GetState());
            await Task.WhenAll(players);
            var sprites = players.Select(player =>
            {
                return new SpriteModel
                {
                    Nickname = player.Result.Nickname,
                    Color = 0x339966,
                    Position = player.Result.Position
                };
            })
            .ToList();

            var gameZoneViewModel = new GameModel
            {
                Player = new PlayerModel { Nickname = nickname },
                Sprites = sprites
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