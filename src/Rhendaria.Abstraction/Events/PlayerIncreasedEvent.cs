namespace Rhendaria.Abstraction.Events
{
    public class PlayerIncreasedEvent : PlayerEventBase
    {
        public PlayerIncreasedEvent(string username, int size) : base(username)
        {
            CurrentSize = size;
        }

        public int CurrentSize { get; }
    }
}