using System;
using System.Collections.Generic;

namespace Rhendaria.Engine.Actors
{
    [Serializable]
    public class ZoneState
    {
        public HashSet<string> Players { get; set; }

        public bool IsEmpty() => Players == null;
    }
}