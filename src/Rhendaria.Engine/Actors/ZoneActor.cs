﻿using System.Collections.Generic;
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
        private readonly ICollisionDetectingService _collisionDetector;
        private readonly IEventBus _eventBus;
        private readonly IScoreCalculatingService _scoreCalculator;

        public ZoneActor(ICollisionDetectingService collisionDetector, IEventBus eventBus, IScoreCalculatingService scoreCalculator)
        {
            _collisionDetector = collisionDetector;
            _eventBus = eventBus;
            _scoreCalculator = scoreCalculator;
        }

        public async Task<Vector2D> RoutePlayerMovement(IPlayerActor player, Direction direction)
        {
            var playerName = await player.GetUsername();
            var currentlyInZone = State.Players.ContainsKey(playerName);

            if (!currentlyInZone)
            {
                State.Players.Add(playerName, player);
                await WriteStateAsync();
            }

            Vector2D currentPosition = await player.Move(direction);

            var opponents = State.Players
                .ExceptOf(playerName)
                .ToList();

            var collissionResult = await _collisionDetector.DetectCollision(player, opponents);

            foreach (var looser in collissionResult.Loosers)
                await _eventBus.PublishPlayerDeadEvent(await looser.GetUsername());

            var score = await _scoreCalculator.CalculateScore(collissionResult.Winner, opponents);

            var winnerName = await collissionResult.Winner.GetUsername();
            await _eventBus.PublishPlayersScoreIncreasedEvent(winnerName, score);

            return currentPosition;
        }

        public override Task OnActivateAsync()
        {
            if (!State.IsInitialized())
                State.Players = new Dictionary<string, IPlayerActor>();

            return Task.CompletedTask;
        }
    }
}