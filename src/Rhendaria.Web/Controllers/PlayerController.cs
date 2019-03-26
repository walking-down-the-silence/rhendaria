using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Orleans;
using Rhendaria.Abstraction.Actors;
using Rhendaria.Abstraction.Services;
using Rhendaria.Web.Commands;
using Rhendaria.Web.Models;
using Rhendaria.Web.Services;
using System.Linq;
using System.Threading.Tasks;
using Rhendaria.Abstraction;

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

            IPlayerActor playerActor = _client.GetGrain<IPlayerActor>(nickname);
            PlayerInfo playerInfo = await playerActor.GetState();

            string zoneId = _routingService.GetZoneId(playerInfo.Position);
            IZoneActor zoneActor = _client.GetGrain<IZoneActor>(zoneId);
            ZoneInfo zoneInfo = await zoneActor.GetPlayers();
            var players = zoneInfo.Players.Select(player => player.GetState());
            PlayerInfo[] playerInfos = await Task.WhenAll(players);

            List<CellModel> sprites = playerInfos
                .Select(player => new CellModel
                {
                    Nickname = player.Nickname,
                    Color = player.SpriteColor,
                    Position = player.Position
                })
                .ToList();

            var gameZoneViewModel = new GameModel
            {
                PlayerCell = new CellModel
                {
                    Nickname = nickname,
                    Color = playerInfo.SpriteColor,
                    Position = playerInfo.Position,
                    Score = playerInfo.SpriteSize
                },
                Cells = sprites
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

            Vector2D playerPosition = await _movementService.MovePlayer(nickname, command.Direction);
            return Ok(playerPosition);
        }
    }
}