namespace Rhendaria.Abstraction
{
    public interface IPlayerActor
    {
        Vector2D GetPosition();

        Vector2D GetSize();

        void Move(Direction direction);
    }
}
