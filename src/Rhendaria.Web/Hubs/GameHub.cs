using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Rhendaria.Abstraction;

namespace Rhendaria.Web.Hubs
{
    public class GameHub : Hub
    {
        private const string UpdatePositionMethod = "UpdatePosition";

        public async Task MovePlayer(string nickname, Direction direction)
        {
            // TODO: execute behavior of moving the player
            // send a notification to a group of players in certain zone that position has been changed
            await Clients.Caller.SendAsync(UpdatePositionMethod, nickname, null);
            await Clients.Group("ZONE_GROUP_ID").SendAsync(UpdatePositionMethod, nickname, null);
        }
    }
}
