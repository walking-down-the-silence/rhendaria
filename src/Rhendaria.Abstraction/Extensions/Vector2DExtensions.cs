namespace Rhendaria.Abstraction.Extensions
{
    public static class Vector2DExtensions
    {
        public static Vector2D Shift(this Vector2D vector, Direction direction)
        {
            switch (direction)
            {
                case Direction.Left:
                    return new Vector2D(vector.Left - 10, vector.Top);
                case Direction.Up:
                    return new Vector2D(vector.Left, vector.Top - 10);
                case Direction.Right:
                    return new Vector2D(vector.Left + 10, vector.Top);
                case Direction.Down:
                    return new Vector2D(vector.Left, vector.Top + 10);
                default:
                    return vector;
            }
        }
    }
}
