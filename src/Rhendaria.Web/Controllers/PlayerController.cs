using Microsoft.AspNetCore.Mvc;
using Orleans;
using Rhendaria.Abstraction;
using System.Threading.Tasks;

namespace Rhendaria.Web.Controllers
{
    [Route("api/[controller]/{username}")]
    [ApiController]
    public class PlayerController : ControllerBase
    {
        private readonly IClusterClient _client;

        public PlayerController(IClusterClient client)
        {
            _client = client;
        }

        [HttpGet("position")]
        public async Task<IActionResult> GetUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                return BadRequest("Username cannot be null or empty.");
            }

            IPlayerActor playerActor = _client.GetGrain<IPlayerActor>(username);
            Vector2D position = await playerActor.GetPosition();

            return Ok(position);
        }

        [HttpPost("move")]
        public async Task<IActionResult> MovePlayer(string username,[FromBody] MovementCommand command)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                return BadRequest("Username cannot be null or empty.");
            }

            IPlayerActor playerActor = _client.GetGrain<IPlayerActor>(username);
            Vector2D position = await playerActor.Move(command.Direction);

            return Ok(position);
        }

        public class MovementCommand
        {
            public Direction Direction { get; set; }
        }
    }
}