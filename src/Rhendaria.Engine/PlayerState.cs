using System;
using Rhendaria.Abstraction;

namespace Rhendaria.Engine
{
    [Serializable]
    public class PlayerState
    {
        public string Color { get; set; }

        public Vector2D Position { get; set; }

        public Vector2D Size { get; set; }
    }
}
