using System;
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

        public Task<int> GetSize()
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

        public override Task OnActivateAsync()
        {
            if (!State.IsInitialized())
            {
                State.Color = "Red";
                State.Position = new Vector2D(0, 0);
                State.Size = 1;
            }

            return Task.CompletedTask;
        }

        public async Task<CollissionCheck> IsCollidedWith(PlayerActor player)
        {
            var position = await GetPosition();
            var targetPosition = await player.GetPosition();

            var dx = position.Left - targetPosition.Left;
            var dy = position.Top - targetPosition.Top;

            var distance = Math.Sqrt(dx * dx + dy * dy);

            var radius = await GetSize();
            var tartedRadius = await player.GetSize();

            var isCollided = distance < radius + tartedRadius;
            return new CollissionCheck
            {
                Player = this,
                IsCollided = isCollided
            };
        }
    }

    public class CollissionCheck
    {
        public PlayerActor Player { get; set; }
        public bool IsCollided { get; set; }
    }
}
