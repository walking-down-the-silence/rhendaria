namespace Rhendaria.Abstraction.Events
{
    public class PlayerDeadEvent : PlayerEventBase
    {
        public PlayerDeadEvent(string username) : base(username)
        {
        }
    }
}