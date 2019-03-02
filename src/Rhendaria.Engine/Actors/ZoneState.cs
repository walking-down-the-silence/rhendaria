using System;
using System.Collections.Generic;

namespace Rhendaria.Engine.Actors
{
    [Serializable]
    public class ZoneState
    {
        public HashSet<string> Players { get; set; }

        public static ZoneState Create()
        {
            return new ZoneState
            {
                Players = new HashSet<string>()
            };
        }

        public bool IsEmpty() => Players == null;
    }
}