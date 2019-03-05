using System;
using Microsoft.Extensions.Options;
using Rhendaria.Abstraction;
using Rhendaria.Abstraction.Services;

namespace Rhendaria.Engine.Services
{
    public class RoutingService : IRoutingService
    {
        private readonly ZoneOptions _zoneOptions;

        public RoutingService(IOptions<ZoneOptions> gameOptions)
        {
            _zoneOptions = gameOptions.Value;
        }

        public string GetZoneId(Vector2D position)
        {
            if (position == null)
            {
                throw new ArgumentNullException(nameof(position));
            }

            int zoneX = (int)position.X / _zoneOptions.ZoneWidth;
            int zoneY = (int)position.Y / _zoneOptions.ZoneHeight;

            return $"zone_{zoneX}_{zoneY}";
        }
    }
}
