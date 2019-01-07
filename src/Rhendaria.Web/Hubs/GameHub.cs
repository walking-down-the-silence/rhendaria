using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Orleans;
using Rhendaria.Abstraction;
using Rhendaria.Abstraction.Actors;

namespace Rhendaria.Web.Hubs
{
    public class GameHub : Hub
    {
        private readonly IClusterClient _client;

        public GameHub(IClusterClient client)
        {
            _client = client;
        }

        public async Task Move(string username, Direction direction)
        {
            var player = _client.GetGrain<IPlayerActor>(username);
            var zone = _client.GetGrain<IZoneActor>("zone");

            Vector2D position = await player.Move(direction);

            await zone.RoutePlayerMovement(player);

            await Clients.All.SendAsync("OnMove", position);
        }
    }
}
