using System.Threading.Tasks;
using Orleans;

namespace Rhendaria.Abstraction.Actors
{
    public interface IPlayerActor : IGrainWithStringKey
    {
        Task<PlayerInfo> GetState();

        Task<Vector2D> Move(Vector2D direction);
    }
}
