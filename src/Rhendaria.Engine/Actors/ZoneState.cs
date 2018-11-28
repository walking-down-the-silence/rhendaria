using System;
using System.Collections.Generic;
using Rhendaria.Abstraction.Actors;

namespace Rhendaria.Engine.Actors
{
    [Serializable]
    public class ZoneState
    {
        public Dictionary<string, IPlayerActor> Players { get; set; }

        public bool IsInitialized()
        {
            return Players != null;
        }
    }
}