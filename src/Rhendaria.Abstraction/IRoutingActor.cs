using Orleans;
using System.Threading.Tasks;

namespace Rhendaria.Abstraction
{
    public interface IRoutingActor : IGrainWithGuidKey
    {
        Task RoutePlayerMovement(string username, Vector2D size, Vector2D position);
    }
}
