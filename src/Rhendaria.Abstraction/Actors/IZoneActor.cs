using System.Threading.Tasks;
using Orleans;

namespace Rhendaria.Abstraction.Actors
{
    public interface IZoneActor : IGrainWithStringKey
    {
        Task<ZoneInfo> GetPlayers();

        Task RoutePlayerMovement(IPlayerActor player);
    }
}
