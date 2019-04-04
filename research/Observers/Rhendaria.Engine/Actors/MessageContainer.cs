using Orleans;
using Rhendaria.Abstraction.Actors;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Rhendaria.Engine.Actors
{
    public class MessageContainer : Grain, IMessageContainer
    {
        private readonly ILogger<MessageContainer> _logger;
        private IClientEventListener _client;

        public MessageContainer(ILogger<MessageContainer> logger)
        {
            _logger = logger;
        }

        public int InnerCounter { get; set; } = 0;

        public Task Subscribe(IClientEventListener clientEventListener)
        {
            _client = clientEventListener;

            return Task.CompletedTask;
        }

        public Task InsertMessage(string message)
        {
            _logger.LogInformation($"Counter: {++InnerCounter}");

            _client.PushMessage($"{InnerCounter}: {message}");

            return Task.CompletedTask;
        }
    }
}
