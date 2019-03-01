using Microsoft.AspNetCore.SignalR;
using Rhendaria.Abstraction;
using Rhendaria.Web.Services;
using System.Threading.Tasks;

namespace Rhendaria.Web.Hubs
{
    public class GameHub : Hub
    {
        private const string UpdatePositionMethod = "UpdatePosition";
        //private readonly PlayerMovementService _movementService;

        //public GameHub(PlayerMovementService movementService)
        //{
        //    _movementService = movementService;
        //}

        public async Task MovePlayer(string nickname, Vector2D direction)
        {
            // TODO: execute behavior of moving the player
            // send a notification to a group of players in certain zone that position has been changed
            //await _movementService.MovePlayer(nickname, direction);
            //await Clients.Caller.SendAsync(UpdatePositionMethod, nickname, null);
            //await Clients.Group("ZONE_GROUP_ID").SendAsync(UpdatePositionMethod, nickname, null);
        }
    }
}
