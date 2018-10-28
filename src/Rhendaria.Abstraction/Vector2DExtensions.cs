namespace Rhendaria.Abstraction
{
    public static class Vector2DExtensions
    {
        public static Vector2D Shift(this Vector2D vector, Direction direction)
        {
            switch (direction)
            {
                case Direction.Left:
                    return new Vector2D(vector.Left - 1, vector.Top);
                case Direction.Up:
                    return new Vector2D(vector.Left, vector.Top - 1);
                case Direction.Right:
                    return new Vector2D(vector.Left + 1, vector.Top);
                case Direction.Down:
                    return new Vector2D(vector.Left, vector.Top + 1);
                default:
                    return vector;
            }
        }
    }
}
