namespace Rhendaria.Abstraction
{
    public interface IRoutingActor
    {
        void RoutePlayerMovement(string username, Vector2D size, Vector2D position);
    }
}
