using System;
using Rhendaria.Abstraction;
using Rhendaria.Abstraction.Services;

namespace Rhendaria.Engine.Services
{
    public class RoutingService : IRoutingService
    {
        private readonly IGameConstants _gameConstants;

        public RoutingService(IGameConstants gameConstants)
        {
            _gameConstants = gameConstants;
        }

        public string GetZoneId(Vector2D position)
        {
            if (position == null) throw new ArgumentNullException(nameof(position));

            int zoneX = position.Left / _gameConstants.ZoneWidth;
            int zoneY = position.Top / _gameConstants.ZoneHeight;

            return $"zone_{zoneX}_{zoneY}";
        }
    }
}
