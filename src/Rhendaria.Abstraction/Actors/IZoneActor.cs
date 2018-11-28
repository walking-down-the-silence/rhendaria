﻿using System.Threading.Tasks;
using Orleans;

namespace Rhendaria.Abstraction.Actors
{
    public interface IZoneActor : IGrainWithStringKey
    {
        Task<Vector2D> RoutePlayerMovement(IPlayerActor player, Direction direction);
    }
}
