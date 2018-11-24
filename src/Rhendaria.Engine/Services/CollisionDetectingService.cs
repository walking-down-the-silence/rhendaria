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
        public Task<CollisionResult> DetectCollision(IPlayerActor player, ICollection<IPlayerActor> opponents)
        {
            var collissionChecks = opponents
                .Select(async opponent => await IsCollidedWith(opponent, player))
                .ToList();

            var collided = collissionChecks
                .Where(x => x.Result.IsCollided)
                .Select(x => x.Result.Player)
                .OrderByDescending(async p => await p.GetSize())
                .ToList();

            var result = new CollisionResult
            {
                Loosers = collided.Skip(1).ToList(),
                Winner = collided.First()
            };

            return Task.FromResult(result);
        }

        private class CollissionCheck
        {
            public IPlayerActor Player { get; set; }
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
                Player = player,
                IsCollided = distance < radius + tartedRadius
            };
        }
    }
}