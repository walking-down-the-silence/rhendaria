using System.Threading.Tasks;
using Orleans;

namespace Rhendaria.Abstraction.Actors
{
    public interface IPlayerActor : IGrainWithStringKey
    {
        Task<string> GetUsername();

        Task<Vector2D> GetPosition();

        Task<int> GetSize();

        Task<Vector2D> Move(Direction direction);
    }
}
