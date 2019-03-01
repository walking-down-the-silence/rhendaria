using static System.Math;

namespace Rhendaria.Abstraction.Extensions
{
    public static class Vector2DExtensions
    {
        public static Vector2D Shift(this Vector2D vector, Direction direction, double distance)
        {
            switch (direction)
            {
                case Direction.Left:
                    return new Vector2D(vector.X - distance, vector.Y);
                case Direction.Up:
                    return new Vector2D(vector.X, vector.Y - distance);
                case Direction.Right:
                    return new Vector2D(vector.X + distance, vector.Y);
                case Direction.Down:
                    return new Vector2D(vector.X, vector.Y + distance);
                default:
                    return vector;
            }
        }

        public static Vector2D Shift(this Vector2D vector, Vector2D direction, double distance)
        {
            var target = new Vector2D(vector.X + direction.X, vector.Y + direction.Y);
            double angle = 0.0;
            double x = vector.X + Cos(angle) * distance;
            double y = vector.Y + Sin(angle) * distance;
            return new Vector2D((int)x, (int)y);
        }
    }
}
