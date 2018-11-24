namespace Rhendaria.Abstraction.Events
{
    public abstract class PlayerEventBase
    {
        protected PlayerEventBase()
        {
        }

        protected PlayerEventBase(string username)
        {
            Username = username;
        }

        public string Username { get; }
    }
}