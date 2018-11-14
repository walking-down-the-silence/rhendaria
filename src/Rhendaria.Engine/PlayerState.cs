using System;
using System.Runtime.Serialization;
using Rhendaria.Abstraction;

namespace Rhendaria.Engine
{
    [Serializable]
    public class PlayerState
    {
        public PlayerState()
        {
            IsInitialized = true;
        }

        public string Color { get; set; }

        public Vector2D Position { get; set; }

        public Vector2D Size { get; set; }

        [IgnoreDataMember]
        public bool IsInitialized { get; }
    }
}
