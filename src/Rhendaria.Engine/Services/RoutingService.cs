using System;
using Rhendaria.Abstraction;
using Rhendaria.Abstraction.Services;

namespace Rhendaria.Engine.Services
{
    public class RoutingService : IRoutingService
    {
        private readonly IGameOptions _gameOptions;

        public RoutingService(IGameOptions gameOptions)
        {
            this._gameOptions = gameOptions;
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
