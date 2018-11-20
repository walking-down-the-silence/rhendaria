using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rhendaria.Abstraction;

namespace Rhendaria.Engine
{
    public class ZoneActor : IZoneActor
    {
        private readonly Dictionary<string, PlayerActor> _players = new Dictionary<string, PlayerActor>();

        public async Task AddNewUser(PlayerActor player)
        {
            var username = await player.GetUsername();
            _players[username] = player;
        }

        public async Task RoutePlayerMovement(string username, Direction direction)
        {
            if (!_players.TryGetValue(username, out var player))
                player = new PlayerActor();

            await player.Move(direction);

            var collissionChecks = _players
                .Select(async p => await p.Value.IsCollidedWith(player))
                .ToList();

            var collided = collissionChecks
                .Where(x => x.Result.IsCollided)
                .Select(x => x.Result.Player)
                .OrderByDescending(async p => await p.GetSize())
                .ToList();

            var winner = collided.First();
            var loosers = collided.Skip(1);
        }

        public async Task RemoveUser(PlayerActor player)
        {
            var username = await player.GetUsername();
            _players.Remove(username);
        }

        public Task RoutePlayerMovement(string username, Vector2D size, Vector2D position)
        {
            throw new System.NotImplementedException();
        }
    }
}