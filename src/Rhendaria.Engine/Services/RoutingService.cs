using System;
using Microsoft.Extensions.Options;
using Rhendaria.Abstraction;
using Rhendaria.Abstraction.Services;

namespace Rhendaria.Engine.Services
{
    public class RoutingService : IRoutingService
    {
        private readonly GameOptions _gameOptions;

        public RoutingService(IOptions<GameOptions> gameOptions)
        {
            this._gameOptions = gameOptions.Value;
        }

        public string GetZoneId(Vector2D position)
        {
            if (position == null) throw new ArgumentNullException(nameof(position));

            int zoneX = position.Left / this._gameOptions.ZoneWidth;
            int zoneY = position.Top / this._gameOptions.ZoneHeight;

            return $"zone_{zoneX}_{zoneY}";
        }
    }
}
