using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rhendaria.Abstraction;
using Rhendaria.Abstraction.Actors;
using Rhendaria.Abstraction.Services;

namespace Rhendaria.Engine.Services
{
    public class CollisionDetectingService : ICollisionDetectingService
    {
        public async Task<CollisionResult> DetectCollision(IPlayerActor player, ICollection<IPlayerActor> opponents)
        {
            var collissionChecks = await Task.WhenAll(opponents
                .Select(opponent => IsCollidedWith(player, opponent)));

            var targets = collissionChecks
                .Where(x => x.IsCollided)
                .Select(x => x.Target);

            var orderedPlayers = (await Task.WhenAll(targets.Append(player)
                .Select(async target => new { Size = await target.GetSize(), Target = target })))
                .OrderByDescending(x => x.Size)
                .Select(x => x.Target)
                .ToList();

            var result = new CollisionResult
            {
                Loosers = orderedPlayers.Skip(1).ToList(),
                Winner = orderedPlayers.FirstOrDefault() ?? player
            };

            return result;
        }

        private class CollissionCheck
        {
            public IPlayerActor Target { get; set; }
            public bool IsCollided { get; set; }
        }

        private static async Task<CollissionCheck> IsCollidedWith(IPlayerActor player, IPlayerActor targetPlayer)
        {
            var position = await player.GetPosition();
            var targetPosition = await targetPlayer.GetPosition();

            var dx = position.Left - targetPosition.Left;
            var dy = position.Top - targetPosition.Top;

            var distance = Math.Sqrt(dx * dx + dy * dy);

            var radius = await player.GetSize();
            var tartedRadius = await targetPlayer.GetSize();

            return new CollissionCheck
            {
                Target = targetPlayer,
                IsCollided = distance < radius + tartedRadius
            };
        }
    }
}