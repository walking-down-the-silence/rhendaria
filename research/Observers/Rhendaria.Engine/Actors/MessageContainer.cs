using Orleans;
using Rhendaria.Abstraction.Actors;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Rhendaria.Engine.Actors
{
    public class MessageContainer : Grain, IMessageContainer
    {
        private readonly ILogger<MessageContainer> _logger;

        public MessageContainer(ILogger<MessageContainer> logger)
        {
            _logger = logger;
        }

        public int InnerCounter { get; set; } = 0;
        public Task InsertMessage()
        {
            _logger.LogInformation($"Counter: {++InnerCounter}");

            return Task.CompletedTask;
        }
    }
}
