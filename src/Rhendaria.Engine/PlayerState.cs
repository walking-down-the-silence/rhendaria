using Rhendaria.Abstraction;
using System;

namespace Rhendaria.Hosting
{
    [Serializable]
    public class PlayerState
    {
        public string Color { get; set; }

        public Vector2D Position { get; set; }

        public Vector2D Size { get; set; }
    }
}
