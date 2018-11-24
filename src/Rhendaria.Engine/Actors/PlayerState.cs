using System;
using Rhendaria.Abstraction;

namespace Rhendaria.Engine.Actors
{
    [Serializable]
    public class PlayerState
    {
        public string Color { get; set; }

        public Vector2D Position { get; set; }

        public int Size { get; set; } = 0;

        public bool IsInitialized()
        {
            return !(string.IsNullOrEmpty(Color) && Position == null && Size == 0);
        }
    }
}
