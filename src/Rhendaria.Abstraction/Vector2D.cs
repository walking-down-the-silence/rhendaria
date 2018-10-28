using System;

namespace Rhendaria.Abstraction
{
    [Serializable]
    public class Vector2D
    {
        public Vector2D(int left, int top)
        {
            Left = left;
            Top = top;
        }

        public int Left { get; }

        public int Top { get; }
    }
}
