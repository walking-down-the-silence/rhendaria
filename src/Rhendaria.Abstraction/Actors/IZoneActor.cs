using System.Threading.Tasks;
using Orleans;

namespace Rhendaria.Abstraction.Actors
{
    public interface IZoneActor : IGrainWithStringKey
    {
        Task RoutePlayerMovement(IPlayerActor player);
    }
}
