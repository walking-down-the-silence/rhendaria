using System;
using System.Runtime.Serialization;
using Rhendaria.Abstraction;

namespace Rhendaria.Engine
{
    [Serializable]
    public class PlayerState
    {
        public string Color { get; set; }

        public Vector2D Position { get; set; }

        public Vector2D Size { get; set; }

        [IgnoreDataMember]
        public bool IsInitialized => !(Color == null && Position == null && Size == null);
    }
}
