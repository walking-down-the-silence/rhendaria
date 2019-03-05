using System.Collections.Generic;
using Rhendaria.Abstraction.Actors;

namespace Rhendaria.Abstraction
{
    public class CollisionResult
    {
        public ICollection<IPlayerActor> Loosers { get; set; }

        public IPlayerActor Winner { get; set; }

        public bool IsEmpty() => Winner == null;
    }
}