namespace Rhendaria.Abstraction.Events
{
    public class PlayerPositionChanged : PlayerEventBase
    {
        public PlayerPositionChanged(string username, Vector2D position) : base(username)
        {
            Position = position;
        }

        public Vector2D Position { get; set; }
    }
}