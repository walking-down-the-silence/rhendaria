using System.Collections.Generic;
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
        private readonly IGrainFactory _grainFactory;
        private readonly ICollisionDetectingService _collisionDetector;
        private readonly IEventBus _eventBus;
        private readonly IScoreCalculatingService _scoreCalculator;

        public ZoneActor(IGrainFactory grainFactory,
            ICollisionDetectingService collisionDetector,
            IScoreCalculatingService scoreCalculator,
            IEventBus eventBus)
        {
            _grainFactory = grainFactory;
            _collisionDetector = collisionDetector;
            _eventBus = eventBus;
            _scoreCalculator = scoreCalculator;
        }

        public async Task RoutePlayerMovement(IPlayerActor player)
        {
            await RegisterPlayerIfRequired(player);

            await HandleColissionsAsync(player);
        }

        private async Task RegisterPlayerIfRequired(IPlayerActor player)
        {
            var playerName = await player.GetUsername();
            var currentlyInZone = State.Players.Contains(playerName);

            if (!currentlyInZone)
            {
                State.Players.Add(playerName);
                await WriteStateAsync();
            }
        }

        private async Task HandleColissionsAsync(IPlayerActor currentPlayer)
        {
            var player = await currentPlayer.GetUsername();
            var opponents = State.Players.ExceptOf(player)
                .Select(opponent => _grainFactory.GetGrain<IPlayerActor>(opponent))
                .ToList();

            var collissionResult = await _collisionDetector.DetectCollision(currentPlayer, opponents);

            if (collissionResult.IsEmpty())
                return;

            var score = await _scoreCalculator.CalculateScore(collissionResult.Winner, collissionResult.Loosers);

            var winner = await collissionResult.Winner.GetUsername();
            await _eventBus.PublishPlayersScoreIncreasedEvent(winner, score);

            foreach (var looser in collissionResult.Loosers)
            {
                var looserName = await looser.GetUsername();
                await _eventBus.PublishPlayerDeadEvent(looserName);

                State.Players.Remove(looserName);
            }

            await WriteStateAsync();
        }

        public override Task OnActivateAsync()
        {
            if (!State.IsInitialized())
                State.Players = new HashSet<string>();

            return Task.CompletedTask;
        }
    }
}