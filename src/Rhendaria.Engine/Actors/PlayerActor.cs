using System.Threading.Tasks;
using Orleans;
using Rhendaria.Abstraction;
using Rhendaria.Abstraction.Actors;
using Rhendaria.Abstraction.Extensions;

namespace Rhendaria.Engine.Actors
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

        public async Task<int> GetSize()
        {
            await Task.Yield();
            return State.Size;
        }

        public async Task<Vector2D> Move(Direction direction)
        {
            Vector2D result = State.Position.Shift(direction);
            State.Position = result;

            await WriteStateAsync();
            return result;
        }

        public override Task OnActivateAsync()
        {
            if (!State.IsInitialized())
            {
                State.Color = "Red";
                State.Position = new Vector2D(0, 0);
                State.Size = 3;
            }
      
            return Task.CompletedTask;
        }
    }
}
