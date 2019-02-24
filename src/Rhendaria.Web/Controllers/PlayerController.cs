using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Orleans;
using Rhendaria.Abstraction;
using Rhendaria.Abstraction.Actors;
using Rhendaria.Web.Commands;
using Rhendaria.Web.Hubs;
using System.Threading.Tasks;

namespace Rhendaria.Web.Controllers
{
    [Route("api/[controller]")]
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

        [HttpPost("{username}/move")]
        public async Task<IActionResult> MovePlayer(string username, [FromBody] MovementCommand command)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                return BadRequest("Username cannot be null or empty.");
            }

            var player = _client.GetGrain<IPlayerActor>(username);
            var zone = _client.GetGrain<IZoneActor>("zone");

            Vector2D position = await player.Move(command.Direction);
            await zone.RoutePlayerMovement(player);
            await _hubContext.Clients.All.SendAsync("UpdatePosition", username, position);

            return Ok(position);
        }
    }
}