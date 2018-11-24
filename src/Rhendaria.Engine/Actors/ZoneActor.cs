using System.Linq;
using System.Threading.Tasks;
using Orleans;
using Rhendaria.Abstraction;
using Rhendaria.Abstraction.Actors;
using Rhendaria.Abstraction.Extensions;
using Rhendaria.Abstraction.Services;

namespace Rhendaria.Engine.Actors
{
    public class ZoneActor : Grain<ZoneState>, IZoneActor
    {
        private readonly IClusterClient _client;
        private readonly ICollisionDetectingService _collisionDetector;
        private readonly IEventBus _eventBus;
        private readonly IScoreCalculatingService _scoreCalculator;

        public ZoneActor(IClusterClient client, ICollisionDetectingService collisionDetector, IEventBus eventBus, IScoreCalculatingService scoreCalculator)
        {
            _client = client;
            _collisionDetector = collisionDetector;
            _eventBus = eventBus;
            _scoreCalculator = scoreCalculator;
        }

        public async Task<Vector2D> RoutePlayerMovement(string username, Direction direction)
        {
            var currentlyInZone = State.Players.Contains(username);

            if (!currentlyInZone)
            {
                State.Players.Add(username);
                await WriteStateAsync();
            }

            var player = _client.GetGrain<IPlayerActor>(username);

            Vector2D currentPosition = await player.Move(direction);

            var opponents = State.Players
                .ExceptOf(username)
                .Select(opponentName => _client.GetGrain<IPlayerActor>(opponentName))
                .ToList();

            var collissionResult = await _collisionDetector.DetectCollision(player, opponents);

            foreach (var looser in collissionResult.Loosers)
                await _eventBus.PublishPlayerDeadEvent(await looser.GetUsername());

            var score = await _scoreCalculator.CalculateScore(collissionResult.Winner, opponents);

            var winnerName = await collissionResult.Winner.GetUsername();
            await _eventBus.PublishPlayersScoreIncreasedEvent(winnerName, score);

            return currentPosition;
        }
    }
}