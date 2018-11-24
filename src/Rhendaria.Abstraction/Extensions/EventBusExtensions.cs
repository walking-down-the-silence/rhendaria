using System.Threading.Tasks;
using Rhendaria.Abstraction.Events;
using Rhendaria.Abstraction.Services;

namespace Rhendaria.Abstraction.Extensions
{
    public static class EventBusExtensions
    {
        public static Task PublishPlayerDeadEvent(this IEventBus eventBus, string player)
        {
            var deathEvent = new PlayerDeadEvent(player); 

            eventBus.Publish(nameof(PlayerDeadEvent), deathEvent);
            return Task.CompletedTask;
        }

        public static Task PublishPlayersScoreIncreasedEvent(this IEventBus eventBus, string player, int size)
        {
            var increaseEvent = new PlayerIncreasedEvent(player, size);

            eventBus.Publish(nameof(PlayerIncreasedEvent), increaseEvent);
            return Task.CompletedTask;
        }
    }
}