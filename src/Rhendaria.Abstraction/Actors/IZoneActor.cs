using System.Threading.Tasks;
using Orleans;

namespace Rhendaria.Abstraction.Actors
{
    public interface IZoneActor : IGrainWithStringKey
    {
        Task<Vector2D> RoutePlayerMovement(string username, Direction direction);
    }
}
