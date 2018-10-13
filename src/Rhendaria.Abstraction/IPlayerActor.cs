namespace Rhendaria.Abstraction
{
    public interface IPlayerActor
    {
        Vector2D GetPosition();

        Vector2D GetSize();

        PlayerInfo GetPlayerInfo();

        void Move(Direction direction);
    }
}
