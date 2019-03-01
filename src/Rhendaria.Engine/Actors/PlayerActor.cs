using Microsoft.Extensions.Options;
using Orleans;
using Rhendaria.Abstraction;
using Rhendaria.Abstraction.Actors;
using Rhendaria.Abstraction.Extensions;
using System.Threading.Tasks;

namespace Rhendaria.Engine.Actors
{
    public class PlayerActor : Grain<PlayerState>, IPlayerActor
    {
        private readonly IOptions<ZoneOptions> _options;

        public PlayerActor(IOptions<ZoneOptions> options)
        {
            _options = options;
        }

        public Task<PlayerInfo> GetState()
        {
            var info = new PlayerInfo
            {
                Nickname = this.GetPrimaryKeyString(),
                Position = State.Position,
                SpriteColor = State.Color,
                SpriteSize = State.Size
            };
            return Task.FromResult(info);
        }

        public async Task<Vector2D> Move(Vector2D direction)
        {
            State.Position = State.Position.Shift(direction, 10);
            await WriteStateAsync();
            return State.Position;
        }

        public override Task OnActivateAsync()
        {
            if (State.IsEmpty())
            {
                State = PlayerState.Create(
                    _options.Value.ZoneWidth,
                    _options.Value.ZoneHeight);
            }

            return Task.CompletedTask;
        }
    }
}
