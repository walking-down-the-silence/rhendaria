using System;
using System.Globalization;
using Rhendaria.Abstraction;

namespace Rhendaria.Engine.Actors
{
    [Serializable]
    public class PlayerState
    {
        public Vector2D Position { get; set; }

        public int Color { get; set; }

        public int Size { get; set; }

        public static PlayerState Create(int maxWidth, int maxHeight)
        {
            var random = new Random();
            var hexColor = string.Format("{0:X6}", random.Next(0x1000000));
            int intColor = int.Parse(hexColor, NumberStyles.HexNumber);
            var position = new Vector2D(random.Next(maxWidth), random.Next(maxHeight));
            return new PlayerState { Color = intColor, Position = position, Size = 50 };
        }

        public bool IsEmpty()
        {
            return Color == 0
                || Position == null
                || Size == 0;
        }
    }
}
