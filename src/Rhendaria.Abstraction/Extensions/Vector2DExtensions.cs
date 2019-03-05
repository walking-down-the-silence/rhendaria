using static System.Math;

namespace Rhendaria.Abstraction.Extensions
{
    public static class Vector2DExtensions
    {
        public static Vector2D Shift(this Vector2D source, Direction direction, double distance)
        {
            switch (direction)
            {
                case Direction.Left:
                    return new Vector2D(source.X - distance, source.Y);
                case Direction.Up:
                    return new Vector2D(source.X, source.Y - distance);
                case Direction.Right:
                    return new Vector2D(source.X + distance, source.Y);
                case Direction.Down:
                    return new Vector2D(source.X, source.Y + distance);
                default:
                    return source;
            }
        }

        public static Vector2D Shift(this Vector2D source, Vector2D direction, double distance)
        {
            var vector = direction.Subtract(source);
            double angle = Vector2D.XAxis.Angle(vector);
            double x = source.X + Cos(angle) * distance;
            double y = source.Y + Sin(angle) * distance * Sign(vector.Y);
            return new Vector2D(x, y);
        }
    }
}
