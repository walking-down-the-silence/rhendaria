using Microsoft.AspNetCore.SignalR;
using Orleans;
using Rhendaria.Abstraction;
using Rhendaria.Abstraction.Actors;
using Rhendaria.Abstraction.Events;
using Rhendaria.Abstraction.Services;
using Rhendaria.Web.Hubs;
using System.Threading.Tasks;

namespace Rhendaria.Web.Services
{
    public class PlayerMovementService
    {
        private const string UpdatePositionMethod = "UpdatePosition";
        private readonly IHubContext<GameHub> _hubContext;
        private readonly IClusterClient _clusterClient;
        private readonly IRoutingService _routingService;

        public PlayerMovementService(
            IHubContext<GameHub> hubContext,
            IClusterClient clusterClient,
            IRoutingService routingService)
        {
            _hubContext = hubContext;
            _clusterClient = clusterClient;
            _routingService = routingService;
        }

        public async Task<Vector2D> MovePlayer(string nickname, Vector2D direction)
        {
            var player = _clusterClient.GetGrain<IPlayerActor>(nickname);
            var position = await player.Move(direction);

            var zone = _clusterClient.GetGrain<IZoneActor>(_routingService.GetZoneId(position));
            await zone.RoutePlayerMovement(player);

            var playerPosition = new PlayerPositionChanged(nickname, position);
            await _hubContext.Clients.All.SendAsync(UpdatePositionMethod, nickname, playerPosition);
            //await Clients.Caller.SendAsync(UpdatePositionMethod, nickname, null);
            //await Clients.Group("ZONE_GROUP_ID").SendAsync(UpdatePositionMethod, nickname, null);

            return position;
        }
    }
}
