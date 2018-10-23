using Microsoft.AspNetCore.Mvc;
using Orleans;
using Rhendaria.Abstraction;
using System.Threading.Tasks;

namespace Rhendaria.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerController : ControllerBase
    {
        private readonly IClusterClient _client;

        public PlayerController(IClusterClient client)
        {
            _client = client;
        }

        [HttpGet("{username}/position")]
        public async Task<IActionResult> GetUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                return BadRequest("Username is ivalid.");
            }

            IPlayerActor playerActor = _client.GetGrain<IPlayerActor>(username);
            Vector2D position = await playerActor.GetPosition();

            return Ok(position);
        }
    }
}