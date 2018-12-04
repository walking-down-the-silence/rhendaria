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

            var collided = collissionChecks
                .Where(x => x.IsCollided)
                .Select(x => x.Target);

            var sortedDictionary = new SortedDictionary<int, List<IPlayerActor>>();
            foreach (var grain in collided.Append(player))
            {
                var size = await grain.GetSize();

                if (!sortedDictionary.ContainsKey(size))
                    sortedDictionary.Add(size, new List<IPlayerActor>());

                sortedDictionary[size].Add(grain);
            }

            var loosers = sortedDictionary
                .Take(sortedDictionary.Count - 1)
                .SelectMany(k => k.Value)
                .ToList();

            var winner = sortedDictionary.Count > 1 ? sortedDictionary.Last().Value.Last() : null;

            var result = new CollisionResult
            {
                Loosers = loosers,
                Winner = winner,
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