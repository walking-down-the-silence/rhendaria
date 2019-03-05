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
                var info = await grain.GetState();

                if (!sortedDictionary.ContainsKey(info.SpriteSize))
                {
                    sortedDictionary.Add(info.SpriteSize, new List<IPlayerActor>());
                }

                sortedDictionary[info.SpriteSize].Add(grain);
            }

            var loosers = sortedDictionary
                .Take(sortedDictionary.Count - 1)
                .SelectMany(k => k.Value)
                .ToList();

            var winner = sortedDictionary.Count > 1 ? sortedDictionary.LastOrDefault().Value?.LastOrDefault() : null;

            var result = new CollisionResult
            {
                Loosers = loosers,
                Winner = winner,
            };

            return result;
        }

        private static async Task<CollissionCheck> IsCollidedWith(IPlayerActor player, IPlayerActor targetPlayer)
        {
            var playerInfo = await player.GetState();
            var targetInfo = await targetPlayer.GetState();

            var position = playerInfo.Position;
            var targetPosition = targetInfo.Position;

            var dx = position.X - targetPosition.X;
            var dy = position.Y - targetPosition.Y;

            var distance = Math.Sqrt(dx * dx + dy * dy);

            var radius = playerInfo.SpriteSize;
            var tartedRadius = targetInfo.SpriteSize;

            return new CollissionCheck
            {
                Target = targetPlayer,
                IsCollided = distance < radius + tartedRadius
            };
        }

        private class CollissionCheck
        {
            public IPlayerActor Target { get; set; }

            public bool IsCollided { get; set; }
        }
    }
}