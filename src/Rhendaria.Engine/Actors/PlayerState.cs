using System;
using Rhendaria.Abstraction;

namespace Rhendaria.Engine.Actors
{
    [Serializable]
    public class PlayerState
    {
        public Vector2D Position { get; set; }

        public string Color { get; set; }

        public int Size { get; set; }

        public static PlayerState Create(int maxWidth, int maxHeight)
        {
            var random = new Random();
            var color = string.Format("{0:X6}", random.Next(0x1000000));
            var position = new Vector2D(random.Next(maxWidth), random.Next(maxHeight));
            return new PlayerState { Color = color, Position = position, Size = 50 };
        }

        public bool IsEmpty()
        {
            return string.IsNullOrEmpty(Color)
                || Position == null
                || Size == 0;
        }
    }
}
