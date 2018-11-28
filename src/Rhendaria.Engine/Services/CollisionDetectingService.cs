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
            var collissionChecks = opponents
                .Select(opponent => IsCollidedWith(player, opponent))
                .ToList();

            var collided = (await Task.WhenAll(collissionChecks))
                .Where(x => x.IsCollided)
                .Select(x => x.Player)
                .ToList();;

            var ordered = (await Task.WhenAll(collided
                    .Select(async target => new {Size = await target.GetSize(), Target = target})))
                .Union(new[] {new {Size = await player.GetSize(), Target = player}})
                .OrderByDescending(x => x.Size)
                .Select(x => x.Target)
                .ToList();

            var result = new CollisionResult
            {
                Loosers = ordered.Skip(1).ToList(),
                Winner = ordered.FirstOrDefault() ?? player
            };

            return result;
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
                Player = targetPlayer,
                IsCollided = distance < radius + tartedRadius
            };
        }
    }
}