using Orleans;
using Rhendaria.Abstraction;
using System.Threading.Tasks;

namespace Rhendaria.Engine
{
    public class PlayerActor : Grain<PlayerState>, IPlayerActor
    {
        public PlayerActor()
        {
        }

        public Task<string> GetUsername()
        {
            return Task.FromResult(this.GetPrimaryKeyString());
        }

        public Task<Vector2D> GetPosition()
        {
            return Task.FromResult(State.Position);
        }

        public Task<Vector2D> GetSize()
        {
            return Task.FromResult(State.Size);
        }

        public async Task<Vector2D> Move(Direction direction)
        {
            Vector2D result = State.Position.Shift(direction);
            State.Position = result;

            await WriteStateAsync();
            return result;
        }
    }
}
