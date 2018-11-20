using System;
using Rhendaria.Abstraction;

namespace Rhendaria.Engine
{
    [Serializable]
    public class PlayerState
    {
        public PlayerState()
        {
            if (IsInitialized())
                return;

            Color = "White";
            Position = new Vector2D(0, 0);
            Size = 0;
        }

        public string Color { get; set; }

        public Vector2D Position { get; set; }

        public int Size { get; set; }

        public bool IsInitialized()
        {
            return !(Color == "White"
                   && Position.Left == 0 && Position.Top == 0
                   && Size == 0);
        }
    }
}
