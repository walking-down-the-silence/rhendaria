using System;

namespace Rhendaria.Abstraction
{
    [Serializable]
    public class Vector2D
    {
        public Vector2D(double x, double y)
        {
            X = x;
            Y = y;
        }

        public double X { get; }

        public double Y { get; }

        public Vector2D Add(Vector2D vector) => new Vector2D(X + vector.X, Y + vector.Y);

        public Vector2D Subtract(Vector2D vector) => new Vector2D(X - vector.X, Y - vector.Y);

        public double Multiply(Vector2D vector) => X * vector.X + Y * vector.Y;

        public double Magnitude() => Math.Sqrt(X * X + Y * Y);

        public Vector2D Scale(double scale) => new Vector2D(X * scale, Y * scale);

        public Vector2D Shrink(double scale) => new Vector2D(X / scale, Y / scale);
    }
}
