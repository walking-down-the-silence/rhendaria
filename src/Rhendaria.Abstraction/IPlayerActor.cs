using Orleans;
using System.Threading.Tasks;

namespace Rhendaria.Abstraction
{
    public interface IPlayerActor : IGrainWithStringKey
    {
        Task<string> GetUsername();

        Task<Vector2D> GetPosition();

        Task<int> GetSize();

        Task<Vector2D> Move(Direction direction);
    }
}
